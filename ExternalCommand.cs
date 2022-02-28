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
    /// <summary>
    /// Revit external command.
    /// </summary>	
    [Transaction(TransactionMode.Manual)]
    sealed partial class $safeitemname$ : IExternalCommand
    {
        /// <summary>
        /// This method implements the external command within 
        /// Revit.
        /// </summary>
        /// <param name="commandData">An ExternalCommandData 
        /// object which contains reference to Application and 
        /// View needed by external command.</param>
        /// <param name="message">Error message can be returned
        /// by external command. This will be displayed only if
        /// the command status was "Failed". There is a limit 
        /// of 1023 characters for this message; strings longer
        /// than this will be truncated.</param>
        /// <param name="elements">Element set indicating 
        /// problem elements to display in the failure dialog. 
        /// This will be used only if the command status was 
        /// "Failed".</param>
        /// <returns>The result indicates if the execution 
        /// fails, succeeds, or was canceled by user. If it 
        /// does not succeed, Revit will undo any changes made 
        /// by the external command.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ResourceManager resourceManager = new ResourceManager(GetType());
            ResourceManager defaultResourceManager = new ResourceManager(typeof(Properties.Resources));

            Result result = Result.Failed;

            try
            {
                UIApplication uiApp = commandData?.Application;
                UIDocument uiDoc = uiApp?.ActiveUIDocument;
                Application app = uiApp?.Application;
                Document doc = uiDoc?.Document;

                /* Wrap all transactions into the transaction 
                 * group. At first we get the transaction group
                 * localized name. */
                var transactionGroupName = RevitUIBuilder.GetResourceString(GetType(), typeof(Properties.Resources), "_transaction_group_name");

                using (var transactionGroup = new TransactionGroup(doc, transactionGroupName))
                {
                    if (TransactionStatus.Started == transactionGroup.Start())
                    {
                        /* Here do your work or the set of 
                         * works... */
                        if (DoWork(commandData, ref message, elements))
                        {
                            if (TransactionStatus.Committed == transactionGroup.Assimilate())
                            {
                                result = Result.Succeeded;
                            }
                        }
                        else
                        {
                            transactionGroup.RollBack();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show(defaultResourceManager.GetString("_Error"), ex.Message);

                result = Result.Failed;
            }
            finally
            {
                resourceManager.ReleaseAllResources();
                defaultResourceManager.ReleaseAllResources();
            }

            return result;
        }
    }
}
