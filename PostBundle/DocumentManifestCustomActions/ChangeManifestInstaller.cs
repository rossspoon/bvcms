//-----------------------------------------------------------------------
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//-----------------------------------------------------------------------

using System;
using System.Configuration.Install;
using System.ComponentModel;
using System.Collections;
using Microsoft.VisualStudio.Tools.Applications;
using System.IO;

namespace ManifestCustomActions
{
    [RunInstaller(true)]
    public class ChangeManifestInstaller
        : Installer
    {
        static readonly Guid SolutionID = new Guid("1ecff67a-3d25-4181-91ce-f54f0e147c7e");

        public override void Install(IDictionary stateSaver)
        {
            string[] nonpublicCachedDataMembers = null;

            Uri deploymentManifestLocation = null;
            if (Uri.TryCreate(
                Context.Parameters["deploymentManifestLocation"],
                UriKind.RelativeOrAbsolute,
                out deploymentManifestLocation) == false)
            {
                throw new InstallException(
                    "The location of the deployment manifest " + 
                    "is missing or invalid.");
            } 
            string documentLocation =
                Context.Parameters["documentLocation"];
            if (String.IsNullOrEmpty(documentLocation))
            {
                throw new InstallException(
                    "The location of the document is missing.");
            }
            string assemblyLocation =
                Context.Parameters["assemblyLocation"];
            if (String.IsNullOrEmpty(assemblyLocation))
            {
                throw new InstallException(
                    "The location of the assembly is missing.");
            }

            string targetLocation = CreateTargetLocation(documentLocation);
            File.Copy(documentLocation, targetLocation);
            if (ServerDocument.IsCustomized(targetLocation))
            {
                ServerDocument.RemoveCustomization(targetLocation);
            }
            ServerDocument.AddCustomization(
                targetLocation,
                assemblyLocation,
                SolutionID, 
                deploymentManifestLocation, 
                true,
                out nonpublicCachedDataMembers);
            stateSaver.Add("targetLocation", targetLocation);
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            string targetLocation = (string)savedState["targetLocation"];
            if (String.IsNullOrEmpty(targetLocation) == false)
            {
                File.Delete(targetLocation);
            }
            base.Uninstall(savedState);
        }

        string CreateTargetLocation(string documentLocation)
        {
            string fileName = Path.GetFileName(documentLocation);
            string myDocuments = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            return Path.Combine(myDocuments, fileName);           
        }
    }
}
