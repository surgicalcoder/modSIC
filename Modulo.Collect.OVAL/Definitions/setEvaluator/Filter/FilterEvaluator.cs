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
using Modulo.Collect.OVAL.SystemCharacteristics;
using sc = Modulo.Collect.OVAL.SystemCharacteristics;
using Modulo.Collect.OVAL.Helpers;
using Modulo.Collect.OVAL.Definitions.variableEvaluator;

namespace Modulo.Collect.OVAL.Definitions.setEvaluator.Filter
{
    public class FilterEvaluator
    {
        private oval_system_characteristics systemCharacteristics;
        private IEnumerable<StateType> ovalDefinitionStates;
        private VariablesEvaluated variables;


        public FilterEvaluator(oval_system_characteristics systemCharacteristics, IEnumerable<StateType> states, VariablesEvaluated variables)
        {
            this.systemCharacteristics = systemCharacteristics;
            this.ovalDefinitionStates = states;
            this.variables = variables;
        }       

        public IEnumerable<sc.ObjectType> ApplyFilter(IEnumerable<sc.ObjectType> objectTypes, string filterValue)
        {
            List<sc.ObjectType> objectAfterFilter = new List<sc.ObjectType>();
            foreach (var objectType in objectTypes)
            {
                objectAfterFilter.AddRange( this.ApplyFilterInObjectType(filterValue, objectType) );
            }
            return objectAfterFilter;
        }

        private List<sc.ObjectType> ApplyFilterInObjectType(string filterValue, sc.ObjectType objectType)
        {
            List<sc.ObjectType> objectTypesAfterFilter = new List<sc.ObjectType>();
            IEnumerable<string> referenceIds = objectType.GetReferenceTypesInString();
            foreach (string id in referenceIds)
            {
                ItemType itemType = this.systemCharacteristics.GetSystemDataByReferenceId(id);
                StateType state = this.GetStateById(filterValue);
                StateTypeComparator comparator = new StateTypeComparator(state, itemType,this.variables);
                if (comparator.IsEquals())
                {
                    RemoveReferenceID(objectType, id);
                }
                else
                {
                    AddObjectTypeInList(objectType, objectTypesAfterFilter);
                }
            }
            return objectTypesAfterFilter;
        }

        private void RemoveReferenceID(sc.ObjectType objectType, string id)
        {
            var refs = objectType.reference.ToList(); refs.RemoveAll(e => e.item_ref == id);
            objectType.reference = refs.ToArray();
        }

        private StateType GetStateById(string stateId)
        {
            StateType state = this.ovalDefinitionStates.SingleOrDefault(obj => obj.id == stateId);
            return state;
        }

        private void AddObjectTypeInList(sc.ObjectType objectType, List<sc.ObjectType> objectTypes)
        {
            sc.ObjectType existingObjectType = objectTypes.SingleOrDefault(obj => obj.id == objectType.id);
            if (existingObjectType == null)
            {
                objectTypes.Add(objectType);
            }
        }

    }
}
