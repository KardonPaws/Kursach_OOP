 #region Namespaces
using System;
using System.Collections.Generic;
using System.Windows;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB.Architecture;
#endregion

namespace Kursach
{
    class App : IExternalApplication
    {
      
        
        public Result OnStartup(UIControlledApplication a)
        {
            string tabName = "Расчет площадей";
            string panelName = "Расчет";
            a.CreateRibbonTab(tabName);
            var panel = a.CreateRibbonPanel(tabName, panelName);
            var NewButton = new PushButtonData("Расчет полезной площади", "Расчет полезной площади", Assembly.GetExecutingAssembly().Location, "Kursach.Command");
            var NewButon = panel.AddItem(NewButton) as PushButton;
            NewButon.ToolTip = "Расчёт полезной и общей площади и подсчет обычных, избыточных, не окруженых и не размещеных помещений.";
            
            Image img = Properties.Resources.Knopka2;
            ImageSource ImgSrc = Convert(img);
            NewButon.LargeImage = ImgSrc;
            NewButon.Image = ImgSrc;



            return Result.Succeeded;
        }

      

        public BitmapImage Convert (Image img)
        {
            using (var memory = new MemoryStream ())
            {
                img.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
