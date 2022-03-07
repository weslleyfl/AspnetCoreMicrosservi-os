using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Basket.API.Infrastructure.PollyHandler
{
    // Ref:
    // https://anthonygiretti.com/2019/03/26/best-practices-with-httpclient-and-retry-policies-with-polly-in-net-core-2-part-2/
    // https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory#configuring-policies-to-use-services-registered-with-di-such-as-iloggert

    public class LoggingRetryHandler
	{
		private static HttpStatusCode[] serverErrors = new HttpStatusCode[] {
			HttpStatusCode.BadGateway,
			HttpStatusCode.GatewayTimeout,
			HttpStatusCode.ServiceUnavailable,
			HttpStatusCode.InternalServerError,
			HttpStatusCode.TooManyRequests,
			HttpStatusCode.RequestTimeout
		};

		private static StatusCode[] gRpcErrors = new StatusCode[] {
			StatusCode.DeadlineExceeded,
			StatusCode.Internal,
			StatusCode.NotFound,
			StatusCode.ResourceExhausted,
			StatusCode.Unavailable,
			StatusCode.Unknown,
			StatusCode.Cancelled
		};

		public static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> GetPolicy = (provider, request) =>
		{
			//Uses Microsoft.Extensions.Http.Polly
			var logger = provider.GetRequiredService<ILogger<LoggingRetryHandler>>();

			return Policy
				.HandleResult<HttpResponseMessage>(r => {
					var grpcStatus = GetStatusCode(r);
					var httpStatusCode = r.StatusCode;

					return (grpcStatus == null && serverErrors.Contains(httpStatusCode)) || // if the server send an error before gRPC pipeline 
						   (httpStatusCode == HttpStatusCode.OK && gRpcErrors.Contains(grpcStatus.Value)); // if gRPC pipeline handled the request (gRPC always answers OK) 
				})
				.Or<HttpRequestException>()
				.WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)), onRetry: (result, timeSpan, retryCount, context) =>
				{
					if (result.Exception != null)
					{
						logger.LogError($"Request failed with {result.Exception.Message}. Retrying...");
					}
					else
					{
						var grpcMsg = GetStatusMessage(result.Result);
						logger.LogWarning($"Request failed with {grpcMsg}. Retrying...");
					}
				});
		};

		private static string GetStatusMessage(HttpResponseMessage response)
		{
			var headers = response?.Headers;
			if (headers == null) return null;

			if (!headers.Contains("grpc-message"))
				return "";

			return headers.GetValues("grpc-message").First();
		}

		private static StatusCode? GetStatusCode(HttpResponseMessage response)
		{
			var headers = response?.Headers;
			if (headers == null) return null;

			if (!headers.Contains("grpc-status") && response.StatusCode == HttpStatusCode.OK)
				return StatusCode.OK;

			if (headers.Contains("grpc-status"))
				return (StatusCode)int.Parse(headers.GetValues("grpc-status").First());

			return null;
		}
	}
}
