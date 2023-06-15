using BindOpen.System.Data;
using BindOpen.System.Data.Meta;
using BindOpen.System.Scoping;
using System;
using System.Runtime.CompilerServices;

namespace BindOpen.System.Hosting.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBdoSettings : IBdoObject, IReferenced, IBdoScoped
    {
        /// <summary>
        /// 
        /// </summary>
        IBdoConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param key="name"></param>
        /// <returns></returns>
        T Get<T>(string name) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param key="name"></param>
        /// <returns></returns>
        object Get(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param key="propertyName"></param>
        /// <returns></returns>
        T GetProperty<T>([CallerMemberName] string propertyName = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param key="defaultValue"></param>
        /// <param key="propertyName"></param>
        /// <returns></returns>
        T GetProperty<T>(T defaultValue, [CallerMemberName] string propertyName = null)
            where T : struct, IConvertible;

        /// <summary>
        /// 
        /// </summary>
        /// <param key="value"></param>
        /// <param key="propertyName"></param>
        void SetProperty(object value, [CallerMemberName] string propertyName = null);
    }
}