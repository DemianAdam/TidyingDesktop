// <copyright file="FormatDictionaryWrapper.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a Wrapper for the <see cref="FormatDictionary"/> class.
    /// </summary>
    internal class FormatDictionaryWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatDictionaryWrapper"/> class.
        /// </summary>
        /// <param name="keyValue">A collection that implements <see cref="IDictionary{TKey, TValue}"/>.</param>
        public FormatDictionaryWrapper(IDictionary<string, string> keyValue)
        {
            this.Keys = keyValue.Keys;
            this.Values = keyValue.Values;
            this.KeyValuePairs = new SortedDictionary<string, string>(keyValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatDictionaryWrapper"/> class. It's the default constructor for JSON deserialization.
        /// </summary>
        /// <param name="keys"><see cref="IEnumerable{T}"/> of keys.</param>
        /// <param name="values"><see cref="IEnumerable{T}"/> of values.</param>
        [JsonConstructor]
        public FormatDictionaryWrapper(IEnumerable<string> keys, IEnumerable<string> values)
        {
            this.Keys = keys;
            this.Values = values;

            IDictionary<string, string> dict = keys.Zip(values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

            this.KeyValuePairs = new SortedDictionary<string, string>(dict);
        }

        /// <summary>
        /// Gets the keys of the <see cref="FormatDictionaryWrapper"/>.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("keys")]
        public IEnumerable<string> Keys { get; private set; }

        /// <summary>
        /// Gets the values of the <see cref="FormatDictionaryWrapper"/>.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("values")]
        public IEnumerable<string> Values { get; private set; }

        /// <summary>
        /// Gets the <see cref="SortedDictionary{TKey, TValue}"/> of the <see cref="FormatDictionaryWrapper"/>.
        /// </summary>
        [JsonIgnore]
        public SortedDictionary<string, string> KeyValuePairs { get; private set; }
    }
}
