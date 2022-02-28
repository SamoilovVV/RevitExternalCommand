using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;

using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using WPF = System.Windows;

using DKC.Jupiter.RevitUITools;

namespace $rootnamespace$
{
    sealed partial class $fileinputname$
    {
        private bool DoWork(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            if (null == commandData)
            {
                throw new ArgumentNullException(nameof(commandData));
            }

            if (null == message)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (null == elements)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            ResourceManager resourceManager = new ResourceManager(GetType());
            ResourceManager defaultResourceManager = new ResourceManager(typeof(Properties.Resources));

            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp?.ActiveUIDocument;
            Application app = uiApp?.Application;
            Document doc = uiDoc?.Document;

            var transactionName = resourceManager.GetString("_transaction_name");

            try
            {
                using (var tr = new Transaction(doc, transactionName))
                {
                    if (TransactionStatus.Started == tr.Start())
                    {

                        // ====================================
                        // TODO: delete these code rows and put
                        // your code here.
                        TaskDialog.Show(resourceManager.GetString(ResourceKeyNames.TaskDialogTitle),
                            string.Format(resourceManager.GetString(ResourceKeyNames.TaskDialogMessage), GetType().Name));
                        // ====================================

                        return TransactionStatus.Committed == tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                /* TODO: Handle the exception here if you need 
                 * or throw the exception again if you need. */
                throw ex;
            }
            finally
            {
                resourceManager.ReleaseAllResources();
                defaultResourceManager.ReleaseAllResources();
            }

            return false;
        }
    }
}
