/*
 * Modulo Open Distributed SCAP Infrastructure Collector (modSIC)
 * 
 * Copyright (c) 2011-2015, Modulo Solutions for GRC.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 *   
 * - Redistributions in binary form must reproduce the above copyright 
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *   
 * - Neither the name of Modulo Security, LLC nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific  prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 * */


using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulo.Collect.OVAL.Definitions.Unix
{
    public partial class file_object : ObjectType
    {
        public override string ComponentString => "file";

        public override IEnumerable<EntitySimpleBaseType> GetEntityBaseTypes()
        {
            try
            {
                IEnumerable<EntitySimpleBaseType> entities = this.Items.OfType<EntitySimpleBaseType>();
                return entities;
            }
            catch (Exception)
            {
                return new List<EntitySimpleBaseType>();
            }
        }

        public Object GetItemValue(ItemsChoiceType3 itemName)
        {
            for (int i = 0; i <= this.ItemsElementName.Count() - 1; i++)
            {
                var currentItemName = this.ItemsElementName[i].ToString();
                var desiredItemName = itemName.ToString();
                if (currentItemName.Equals(desiredItemName))
                    return this.Items[i];
                
            }

            return null;
        }

        public bool IsFilePathSet()
        {
            return this.GetItemValue(ItemsChoiceType3.filepath) != null;
        }

        public bool HasVariableDefined()
        {
            if (this.IsFilePathSet())
                return !string.IsNullOrEmpty(this.getFilepathEntity().var_ref);

            var pathEntity = this.getPathEntity();
            var isThereVarReferenceInPathEntity = !string.IsNullOrEmpty(pathEntity.var_ref);
            var filenameEntity = this.getFilenameEntity();

            if (filenameEntity == null)
                return isThereVarReferenceInPathEntity;

            return (isThereVarReferenceInPathEntity || (!string.IsNullOrEmpty(filenameEntity.var_ref)));
        }

        public String GetFullFilepath()
        {
            if (this.IsFilePathSet())
                return this.getFilepathEntity().Value;

            var pathValue = this.getPathEntity().Value;
            var filenameValue = getFilenameEntity() == null ? string.Empty : getFilenameEntity().Value;

            if (!pathValue.EndsWith("/"))
                pathValue += "/";

            return $"{pathValue}{filenameValue}";
        }

        private EntityObjectStringType getFilepathEntity()
        {
            return (EntityObjectStringType)this.GetItemValue(ItemsChoiceType3.filepath);
        }

        private EntityObjectStringType getPathEntity()
        {
            return (EntityObjectStringType)this.GetItemValue(ItemsChoiceType3.path);
        }

        private EntityObjectStringType getFilenameEntity()
        {
            return (EntityObjectStringType)this.GetItemValue(ItemsChoiceType3.filename);
        }
    }
}
