﻿/*
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
using System.Text;
using Modulo.Collect.OVAL.Definitions;
using Modulo.Collect.OVAL.SystemCharacteristics;
using systemCharacteristics = Modulo.Collect.OVAL.SystemCharacteristics;
using System.Reflection;
using Modulo.Collect.OVAL.Definitions.VariableEvaluators.Evaluators.LocalVariableComponents;

namespace Modulo.Collect.OVAL.Definitions.VariableEvaluators.Evaluators.LocalVariableComponents
{
    public class LocalVariableObjectComponent : LocalVariableComponent
    {

        private ObjectComponentType ObjectComponent;
        private oval_system_characteristics SystemCharacteristics;

        public LocalVariableObjectComponent(ObjectComponentType objectComponent, oval_system_characteristics systemCharacteristics)
        {
            this.ObjectComponent = objectComponent;
            this.SystemCharacteristics = systemCharacteristics;
        }

        /// <summary>
        /// Get value of the object defined in the object_ref property of ObjectComponentType.
        /// For this type of component, is necessary find the values in the systemCharacteristics.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetValue()
        {
            return this.GetAValueOfObjectInTheSystemCharacteristics(ObjectComponent.object_ref, ObjectComponent.item_field, ObjectComponent.record_field);
        }

        /// <summary>
        /// Gets the a value of object in the system characteristics.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private IEnumerable<string> GetAValueOfObjectInTheSystemCharacteristics(string objectId, string propertyName, string recordFieldname)
        {
            var values = new List<string>();
            if (SystemCharacteristics != null)
            {
                var objectType = SystemCharacteristics.collected_objects.SingleOrDefault(obj => obj.id == objectId);
                if ((objectType != null) && (objectType.reference != null))
                {
                    foreach (var reference in objectType.reference)
                    {
                        var itemType = SystemCharacteristics.system_data.Single(item => item.id == reference.item_ref);

                        if (string.IsNullOrEmpty(recordFieldname))
                        {
                            var itemEntityValue = GetValueFromProperty(itemType, propertyName);
                            values.AddRange(itemEntityValue);
                        }
                        else
                        {
                            var itemEntityValue = GetValueFromRecordField(itemType, propertyName, recordFieldname);
                            values.AddRange(itemEntityValue);
                        }
                    }
                }
            }
            return values;
        }

        private IEnumerable<string> GetValueFromRecordField(ItemType itemType, string propertyName, string recordFieldname)
        {
            var values = new List<String>();
            var property = this.GetPropertyByName(itemType, propertyName, itemType.GetType());

            if (property is EntityItemRecordType)
            {
                foreach (var recordField in ((EntityItemRecordType)property).field)
                    if (recordField.name.Equals(recordFieldname, StringComparison.InvariantCultureIgnoreCase))
                        values.Add(recordField.Value);
            }
            else if (property is EntityItemRecordType[])
            {
                foreach (var prop in (EntityItemRecordType[])property)
                    foreach (var recordField in prop.field)
                        if (recordField.name.Equals(recordFieldname, StringComparison.InvariantCultureIgnoreCase))
                            values.Add(recordField.Value);
            }

            return values;
        }

        /// <summary>
        /// Gets the value from property. The property is defined by the item_ref property of ObjectComponentType.
        /// </summary>
        /// <param name="itemtype">The itemtype.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private IEnumerable<string> GetValueFromProperty(ItemType itemtype, string propertyName)
        {
            var property = this.GetPropertyByName(itemtype, propertyName, itemtype.GetType());

            var values = new List<String>();
            if (property is EntityItemSimpleBaseType)
            {
                var propertyValue = GetPropertyByName(property, "Value", typeof(EntityItemSimpleBaseType));
                if (propertyValue != null)
                    values.Add(propertyValue.ToString());
            }
            else if (property is EntityItemSimpleBaseType[])
            {
                var propertyValues = GetValuesOfArrayProperty(property);
                if (propertyValues != null)
                    values.AddRange(propertyValues);
            }

            return values;
        }

        private object GetPropertyByName(Object obj, string propertyName, Type type)
        {
            object value = null;
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                FieldInfo field = type.GetField(propertyName);
                value = field.GetValue(obj);
            }
            else
            {
                value = property.GetValue(obj, null);
            }            
            return value;
        }

        private List<string> GetValuesOfArrayProperty(object obj)
        {
            List<String> values = new List<string>();
            if (obj is object[])
            {
                foreach (object value in (object[])obj)
                {
                    values.Add(this.GetPropertyByName(value,"Value",value.GetType()).ToString());
                }
            }
            return values;
        }
        
    }
}
