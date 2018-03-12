﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Sample.Infrastructure.Remoting.Communication;

namespace Sample.Infrastructure.Remoting.Client
{
    internal class ServiceProxy<TInterface> : DispatchProxy
    {
        private readonly RemoteProcedureExecutor<RemoteRequest, RemoteResponse> _executor;

        public ServiceProxy(RemoteProcedureExecutor<RemoteRequest, RemoteResponse> executor)
        {
            _executor = executor;
        }

        protected override object Invoke(MethodInfo method, object[] args)
        {
            if (!typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                throw new InvalidOperationException("ServiceProxy supports only asynchronous methods.");
            }

            if (!method.ReturnType.IsGenericType)
            {
                throw new InvalidOperationException("ServiceProxy doesn't support VOID methods.");
            }

            var request = new RemoteRequest(method.Name, args);
            var routingKey = this.GetRoutingKey(method.Name);
            var response = this._executor.Execute(request, routingKey).Result;
            var convertedResponse = Convert.ChangeType(response.Response, method.ReturnType.GetGenericArguments().First());
            return Task.FromResult(convertedResponse); //TODO steal more code from retention tool if necessary
        }

        private string GetRoutingKey(string methodName)
        {
            return $"{typeof(TInterface).Name}.{methodName}";
        }
    }
}
