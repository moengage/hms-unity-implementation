/*
 * Copyright (c) 2014-2020 MoEngage Inc.
 *
 * All rights reserved.
 *
 *  Use of source code or binaries contained within MoEngage SDK is permitted only to enable use of the MoEngage platform by customers of MoEngage.
 *  Modification of source code and inclusion in mobile apps is explicitly allowed provided that all other conditions are met.
 *  Neither the name of MoEngage nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *  Redistribution of source code or binaries is disallowed except with specific prior written permission. Any such redistribution must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;

namespace MoEngage
{
	/// <summary>
	/// Class to build event attributes for tracking an event.
	/// </summary>
	public class Properties
	{
		public Dictionary<string, object> GeneralAttributes { get; }
		public Dictionary<string, Dictionary<string, double>> LocationAttributes { get; }
		public Dictionary<string, string> DateTimeAttributes { get; }
		public bool IsNonInteractive;

		/// <summary>
		/// Creates an instance of Properties Class.
		/// </summary>
		public Properties()
		{
			GeneralAttributes = new Dictionary<string, object>();
			LocationAttributes = new Dictionary<string, Dictionary<string, double>>();
			DateTimeAttributes = new Dictionary<string, string>();
			IsNonInteractive = false;
		}

		/// <summary>
		/// Adds an Integer attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddInteger(string key, int value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			GeneralAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds an boolean attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddBoolean(string key, bool value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			GeneralAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds a double attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddDouble(string key, double value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			GeneralAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds a long attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddLong(string key, long value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			GeneralAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds a string attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddString(string key, string value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			GeneralAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds an ISO Date attribute.
		/// ISO Date Format: yyyy-MM-dd'T'HH:mm:ss.fff'Z'
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="value">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddISODateTime(string key, string value)
		{
			if (IsAttributeNameEmpty(key)) return this;
			DateTimeAttributes.Add(key, value);
			return this;
		}

		/// <summary>
		/// Adds a location attribute.
		/// </summary>
		/// <param name="key">Attribute Name</param>
		/// <param name="location">Attribute Value</param>
		/// <returns>Instance of Properties</returns>
		public Properties AddLocation(string key, GeoLocation location)
		{
			if (IsAttributeNameEmpty(key)) return this;
			LocationAttributes.Add(key, location.ToDictionary());
			return this;
		}

		/// <summary>
		/// Marks an event as non-interactive.
		/// </summary>
		/// <returns>Instance of Properties</returns>
		public Properties SetNonInteractive()
		{
			IsNonInteractive = true;
			return this;
		}

		public bool GetIsNonInteractive()
		{
			return IsNonInteractive;
		}

		private bool IsAttributeNameEmpty(string attrKey)
		{
			return string.IsNullOrWhiteSpace(attrKey);
		}

		public Dictionary<string, object> ToDictionary()
		{
			Dictionary<string, object> propertiesDict = new Dictionary<string, object>();
			propertiesDict.Add(MoEConstants.ARGUMENT_GENERAL_EVENT_ATTRIBUTES, GeneralAttributes);
			propertiesDict.Add(MoEConstants.ARGUMENT_LOCATION_EVENT_ATTRIBUTES, LocationAttributes);
			propertiesDict.Add(MoEConstants.ARGUMENT_TIMESTAMP_EVENT_ATTRIBUTES, DateTimeAttributes);

			return propertiesDict;
		}
	}
}
