namespace ManagPassWord
{
    public static class Resolver
    {
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
    }
}
