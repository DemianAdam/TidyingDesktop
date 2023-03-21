// <copyright file="FormatDictionary.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a collection of keys and values. Where the values are a unique collection.
    /// </summary>
    public class FormatDictionary : IDictionary<string, string>, INotifyCollectionChanged
    {
        private IDictionary<string, string> dict;

        private SortedSet<string> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatDictionary"/> class.
        /// </summary>
        public FormatDictionary()
        {
            this.types = new SortedSet<string>();
            this.dict = new SortedDictionary<string, string>();
        }

        /// <inheritdoc cref="INotifyCollectionChanged.CollectionChanged"/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <inheritdoc cref="INotifyCollectionChanged.CollectionChanged"/>
        public event NotifyCollectionChangedEventHandler? SetChanged;

        /// <summary>
        /// Gets a collection containing the keys in the <see cref="FormatDictionary"/>.
        /// </summary>
        public ICollection<string> Keys { get => this.dict.Keys; }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="FormatDictionary"/>.
        /// </summary>
        public ICollection<string> Values { get => this.dict.Values; }

        /// <summary>
        /// Gets a collection containing the types in the <see cref="FormatDictionary.types"/>.
        /// </summary>
        public ICollection<string> Types { get => this.types; }

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> containing all <see cref="FormatDictionary"/> Value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> ExtensionFormatPair { get => this.dict; }

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="FormatDictionary"/>.
        /// </summary>
        public int Count => this.dict.Count;

        /// <summary>
        /// Gets a value indicating whether <see cref="FormatDictionary"/> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <returns>
        ///     <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
        ///     If the value is not found in <see cref="types"/> throws an <see cref="ArgumentException"/>.
        /// </returns>
        /// <inheritdoc/>
        /// <exception cref="ArgumentException">If a invalid value passed.</exception>
        public string this[string key]
        {
            get => this.dict[key];
            set
            {
                this.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the element with the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element associated with the specified element. if the index is outside of bounds of the collection throws an <see cref="IndexOutOfRangeException"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">If the index is outside of bounds of the collection.</exception>
        public KeyValuePair<string, string> this[int index]
        {
            get
            {
                int count = 0;
                foreach (var item in this.dict)
                {
                    if (index == count)
                    {
                        return item;
                    }

                    count++;
                }

                throw new IndexOutOfRangeException();
            }
        }

        /// <param name="key"><inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)" path="/param[@name='key']"/></param>
        /// <param name="value"><inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)" path="/param[@name='value']"/></param>
        /// <summary>
        ///     <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
        ///     If the value is not found in <see cref="types"/> throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks> <see cref="string"/> <paramref name="value"/> is converted to uppercase.</remarks>
        /// <inheritdoc/>
        public void Add(string key, string value)
        {
            key = key.ToLower();
            value = value.ToUpper();

            if (!this.types.Contains(value))
            {
                throw new ArgumentException($"Value not found in the list of Values: [{value}].");
            }

            if (!this.ValidateExtension(key))
            {
                throw new ArgumentException($"Extension needs to start with a punctuation mark [.]: [{key}].");
            }

            this.dict.TryAdd(key, value);
            List<string> list = new List<string>
                {
                    value,
                    key,
                };
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
        }

        /// <summary>
        ///     <inheritdoc cref="ICollection{T}.Add(T)"/>
        ///     If the <paramref name="item"/>.Values is not found in <see cref="types"/> throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <inheritdoc/>
        /// <remarks> <see cref="string"/> <paramref name="item"/>.Values is converted to uppercase.</remarks>
        public void Add(KeyValuePair<string, string> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc cref="HashSet{T}.Add(T)"/>.
        /// <remarks> <see cref="string"/> <paramref name="item"/> is converted to uppercase.</remarks>
        public bool AddType(string item)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(item))
            {
                if (this.types.Add(item.ToUpper()))
                {
                    this.SetChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                    result = true;
                }
            }

            return result;
        }

        /// <inheritdoc cref="Dictionary{TKey,TValue}.Clear()"/>
        public void Clear()
        {
            this.dict.Clear();
            this.types.Clear();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return this.dict.Contains(item);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>
        public bool ContainsKey(string key)
        {
            return this.dict.ContainsKey(key);
        }

        /// <summary>
        /// Renames a Key of the dictionary.
        /// </summary>
        /// <param name="fromKey">Key to rename.</param>
        /// <param name="toKey">Result key.</param>
        /// <param name="format">Result format.</param>
        /// <returns><see langword="true"/> If the key is renamed successfully. Otherwise <see langword="false"/>.</returns>
        /// <exception cref="KeyNotFoundException">If the key is not found in the dictionary.</exception>
        public bool UpdateElement(string fromKey, string toKey, string format)
        {
            bool result;

            if (!this.dict.ContainsKey(fromKey))
            {
                throw new KeyNotFoundException();
            }

            if (!this.ValidateExtension(toKey))
            {
                throw new ArgumentException($"Extension needs to start with a punctuation mark [.]: [{toKey}].");
            }

            result = this.Remove(fromKey.ToLower());
            this.Add(toKey, format);

            return result;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            this.dict.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.GetEnumerator"/>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        public bool Remove(string key)
        {
            bool result = false;

            if (this.dict.Remove(key))
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key));
                result = true;
            }

            return result;
        }

        /// <inheritdoc cref="ICollection{T}.Remove(T)"/>
        public bool Remove(KeyValuePair<string, string> item)
        {
            bool result = false;

            if (this.dict.Remove(item))
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                result = true;
            }

            return result;
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            return this.dict.TryGetValue(key, out value);
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        /// <summary>
        /// Validate if a string is a extension.
        /// </summary>
        /// <param name="ext">The string to validate.</param>
        /// <returns><see langword="true"/> If the string is a valid extension. Otherwise <see langword="false"/>.</returns>
        private bool ValidateExtension(string ext)
        {
            return ext[0] == '.';
        }
    }
}