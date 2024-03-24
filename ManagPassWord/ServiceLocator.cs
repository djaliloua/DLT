using ManagPassWord.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ManagPassWord
{
    public static class ServiceLocator
    {
        public static T RunMethod<T>(ContentPage page, string methodName) 
        {
            try
            {
                T model = page.Handler.MauiContext.Services.GetRequiredService<T>();
                Type type = model.GetType();
                MethodInfo info = type.GetMethod(methodName);
                T res = (T)info.Invoke(model, null);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
        public static T RunMethod<T>(string methodName) where T:BaseViewModel
        {
            try
            {
                T model = Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
                Type type = model.GetType();
                MethodInfo info = type.GetMethod(methodName);
                T res = (T)info.Invoke(model, null);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
