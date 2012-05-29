using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace KissDTO
{
    public static class KissDTO
    {
        private static readonly Hashtable createInstanceInvokers = new Hashtable();
        private delegate object CreateInstanceInvoker();
        private static readonly Hashtable getAndSetValuesInvokers = new Hashtable();
        private delegate void GetSetValuesInvoker(object source, object target);

        public static TDestination As<TDestination>(this object obj) where TDestination : class
        {
            var instance = FastCreateInstance(typeof(TDestination)) as TDestination;
            obj.GetType().GetProperties().FastCopyValues(obj, instance);

            return instance;
        }

        private static object FastCreateInstance(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var invoker = (CreateInstanceInvoker)createInstanceInvokers[type];
            if (invoker == null)
            {
                LambdaExpression e = Expression.Lambda(typeof(CreateInstanceInvoker), Expression.New(type), null);
                invoker = (CreateInstanceInvoker)e.Compile();
                createInstanceInvokers[type] = invoker;

            }
            return invoker();
        }

        private static void FastCopyValues(this IEnumerable<PropertyInfo> property, object source, object target)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (source == null)
                throw new ArgumentNullException("source");

            if (target == null)
                throw new ArgumentNullException("target");

            GetSetValuesInvoker invoker = GetGetAndSetCachedInvoker(property, source.GetType(), target.GetType());
            invoker(source, target);
        }

        private static GetSetValuesInvoker GetGetAndSetCachedInvoker(IEnumerable<PropertyInfo> properties, Type sourceType, Type targetType)
        {
            var invoker = (GetSetValuesInvoker)getAndSetValuesInvokers[properties];
            if (invoker == null)
            {
                var method = new DynamicMethod("CopyProperties", null, new[] { typeof(object), typeof(object) }, typeof(object), true);
                ILGenerator il = method.GetILGenerator();
                il.DeclareLocal(sourceType);
                il.DeclareLocal(targetType);

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldarg_0);
                if (sourceType.IsClass)
                    il.Emit(OpCodes.Castclass, sourceType);
                else
                    il.Emit(OpCodes.Unbox_Any, sourceType);
                il.Emit(OpCodes.Stloc_0);

                il.Emit(OpCodes.Ldarg_1);
                if (targetType.IsClass)
                    il.Emit(OpCodes.Castclass, targetType);
                else
                    il.Emit(OpCodes.Unbox_Any, targetType);
                il.Emit(OpCodes.Stloc_1);

                foreach (PropertyInfo property in properties)
                {
                    PropertyInfo sourceProperty = sourceType.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetProperty);
                    PropertyInfo targetProperty = targetType.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetProperty);

                    if (sourceProperty != null && targetProperty != null)
                    {
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.EmitCall(OpCodes.Callvirt, sourceProperty.GetGetMethod(true), null);
                        il.EmitCall(OpCodes.Callvirt, targetProperty.GetSetMethod(true), null);
                        il.Emit(OpCodes.Nop);
                    }
                }
                il.Emit(OpCodes.Ret);

                invoker = (GetSetValuesInvoker)method.CreateDelegate(typeof(GetSetValuesInvoker));
                getAndSetValuesInvokers[properties] = invoker;
            }

            return invoker;
        }

    }
}
