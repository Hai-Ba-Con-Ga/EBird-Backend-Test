using System.Reflection;
using AutoMapper;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Auth;
using EBird.Application.Model.Bird;
using EBird.Application.Model.BirdType;
using EBird.Application.Model.Group;
using EBird.Application.Model.Notification;
using EBird.Application.Model.NotificationType;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Rule;
using EBird.Domain.Entities;

namespace EBird.Api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ApplyMappingsFromAssembly(Assembly.Load("EBird.Application"));
        }

        private void ApplyMappingsFromAssembly(params Assembly[] assemblies)
        {

            var mapFromType = typeof(IMapFrom<>);
            var mapToType = typeof(IMapTo<>);

            var mapFromMethodName = nameof(IMapFrom<object>.MappingFrom);
            var mapToMethodName = nameof(IMapTo<object>.MappingTo);

            bool HasMapFromInterface(Type t) => t.IsGenericType && (t.GetGenericTypeDefinition() == mapFromType);
            bool HasMapToInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapToType;
            bool HasInterfaces(Type t) => HasMapFromInterface(t) || HasMapToInterface(t);

            List<Type> typeList = new List<Type>();

            foreach (var assembly in assemblies)
            {
               var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(HasInterfaces)).ToList();
                typeList.AddRange(types);
            }

            foreach (var type in typeList)
            {
                var instance = Activator.CreateInstance(type);

                var methodMappingFromInfo = type.GetMethod(mapFromMethodName) ??
                             instance!.GetType().GetInterface(mapFromType.Name)?.GetMethod(mapFromMethodName);

                var methodMappingToInfo = type.GetMethod(mapToMethodName) ??
                             instance!.GetType().GetInterface(mapToType.Name)?.GetMethod(mapToMethodName);

                methodMappingFromInfo?.Invoke(instance, new object[] { this });
                methodMappingToInfo?.Invoke(instance, new object[] { this });
            }
        }

    }
}
