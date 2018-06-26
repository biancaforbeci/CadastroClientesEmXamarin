using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class LocalFileListViewModel : BindableBase
	{
        public LocalFileListViewModel()
        {
            Title = "Importar e Exportar JSON";
            ImportTitle = "Importar";
            ExportTitle = "Exportar";
            Import = new DelegateCommand(ImportList);
            Export = new DelegateCommand(ExportList);
            Image= "lista.png";
        }

        public string ImportTitle { get; set; }
        public string ExportTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public DelegateCommand Import { get; set; }
        public DelegateCommand Export { get; set; }


        private void ImportList()
        {

        }

        private void ExportList()
        {

        }
    }
}
