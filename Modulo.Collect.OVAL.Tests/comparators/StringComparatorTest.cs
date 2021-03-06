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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulo.Collect.OVAL.Common;
using Modulo.Collect.OVAL.Common.comparators;

namespace Modulo.Collect.OVAL.Tests.comparators
{
    /// <summary>
    /// Summary description for StringComparatorTest
    /// </summary>
    [TestClass]
    public class StringComparatorTest
    {
        public StringComparatorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_equals_operator()
        {   
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare("risk manager", "risk manager", OperationEnumeration.equals);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare("40", "41", OperationEnumeration.equals);
            Assert.IsFalse(compareResult, "the result of comparator is not expected");
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_equals_operator_with_case_sensitive_form()
        {
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"SOFTWARE\Modulo", OperationEnumeration.equals);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"Software\Modulo", OperationEnumeration.equals);
            Assert.IsFalse(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\MoDulo", @"SOFTWARE\Modulo", OperationEnumeration.equals);
            Assert.IsFalse(compareResult, "the result of comparator is not expected");           
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_case_insensitive_equals()
        {
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"SOFTWARE\Modulo", OperationEnumeration.caseinsensitiveequals);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"Software\Modulo", OperationEnumeration.caseinsensitiveequals);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\MoDulo", @"SOFTWARE\Modulo", OperationEnumeration.caseinsensitiveequals);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");           
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_not_equals_comparator()
        {
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare("risk manager", "risk managerNG", OperationEnumeration.notequal);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare("40", "41", OperationEnumeration.notequal);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_not_equal_operator_with_case_sensitive_form()
        {

            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare("risk managerng", "risk managerNG", OperationEnumeration.notequal);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"Software\Modulo", OperationEnumeration.notequal);
            Assert.IsTrue(compareResult, "the result of comparator is not expected");
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_two_strings_using_case_insensitive_not_equals()
        {
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare("risk managerng", "risk managerNG", OperationEnumeration.caseinsensitivenotequal);
            Assert.IsFalse(compareResult, "the result of comparator is not expected");

            compareResult = comparator.Compare(@"SOFTWARE\Modulo", @"Software\Modulo", OperationEnumeration.caseinsensitivenotequal);
            Assert.IsFalse(compareResult, "the result of comparator is not expected");
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_compare_a_string_against_regex_pattern()
        {
            StringComparator comparator = new StringComparator();

            bool compareResult = comparator.Compare("Version", "V.*", OperationEnumeration.patternmatch);
            Assert.IsTrue(compareResult, "the result of comparator is no expected for pattern match operation");

            compareResult = comparator.Compare("Direct", "^Direct.*", OperationEnumeration.patternmatch);
            Assert.IsTrue(compareResult, "the result of comparator is no expected for pattern match operation");

            compareResult = comparator.Compare("Direct Version", "^Direct.*", OperationEnumeration.patternmatch);
            Assert.IsTrue(compareResult, "the result of comparator is no expected for pattern match operation");
        }
    }
}
