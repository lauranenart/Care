using AspectInjector.Broker;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;

namespace CareAPI.Helpers
{
    [Injection(typeof(TraceAspect))]
    public sealed class MethodLogger : Attribute
    {
    }

    [Aspect(Scope.Global, Factory = typeof(AspectFactory))]
    public sealed class TraceAspect
    {
        private readonly ILogger _logger;
        public TraceAspect(ILogger logger)
        {
            _logger = logger;
        }

        [Advice(Kind.Around, Targets = Target.Method)]
        public object Trace(
           [Argument(Source.Type)] Type type,
           [Argument(Source.Name)] string name,
           [Argument(Source.Target)] Func<object[], object> methodDelegate,
           [Argument(Source.Arguments)] object[] args)
        {
            var result = methodDelegate(args);
            _logger.Information($"Method {type.Name}/{name} used.");
            return result;
        }
    }

    public class AspectFactory
    {
        public static object GetInstance(Type aspectType)
        {
            // Here you can implement any instantiation approach, this one is shown for the sake of simplicity
            if (aspectType == typeof(TraceAspect))
            {
                var logger = Log.Logger;
                return new TraceAspect(logger);
            }
            throw new ArgumentException($"Unknown aspect type '{aspectType.FullName}'");
        }
    }
}
