using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppClientes.Droid.PlatformCode;
using AppClientes.Infra;
using Prism;
using Prism.Ioc;

namespace AppClientes.Droid
{
    class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IFileSystem, AndroidFileSystem>();
        }
    }
}