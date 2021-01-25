using System;

namespace Beedux.Chat.Core
{
    /// <summary>
    ///     Represents a globally unique identifier (GUID) with a short(er) string value.
    /// </summary>
    public struct ShortGuid
    {
        /// <summary>
        ///     A read-only instance of the ShortGuid class whose value is guaranteed to be all zeroes.
        /// </summary>
        public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

        private Guid _guid;
        private string _value;

        /// <summary>
        ///     Creates a ShortGuid from a base64 encoded string.
        /// </summary>
        /// <param name="value">
        ///     The encoded guid as a base64 string.
        /// </param>
        public ShortGuid(string value)
        {
            _value = value;
            _guid = Decode(value);
        }

        /// <summary>
        ///     Creates a ShortGuid from a Guid.
        /// </summary>
        /// <param name="guid">The Guid to encode.</param>
        public ShortGuid(Guid guid)
        {
            _value = Encode(guid);
            _guid = guid;
        }

        #region Properties

        public Guid Guid
        {
            get => _guid;
            set
            {
                if (value == _guid)
                    return;

                _guid = value;
                _value = Encode(value);
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (value == _value)
                    return;

                _value = value;
                _guid = Decode(value);
            }
        }

        #endregion

        #region Methods

        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        /// <summary>
        ///     Creates a new instance of a Guid using the string value, then returns the base64 encoded version of the Guid.
        /// </summary>
        /// <param name="value">An actual Guid string (i.e.: not a ShortGuid)</param>
        public static string Encode(string value)
        {
            return Encode(new Guid(value));
        }

        /// <summary>
        ///     Encodes the given Guid as a base64 string that is 22 characters long.
        /// </summary>
        /// <param name="guid">The Guid to encode.</param>
        public static string Encode(Guid guid)
        {
            var encoded = Convert.ToBase64String(guid.ToByteArray());

            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        /// <summary>
        ///     Decodes the given base64 string.
        /// </summary>
        /// <param name="value">The base64 encoded string of a Guid.</param>
        /// <returns>A new Guid.</returns>
        public static Guid Decode(string value)
        {
            value = value
                .Replace("_", "/")
                .Replace("-", "+");

            var buffer = Convert.FromBase64String(value + "==");

            return new Guid(buffer);
        }

        #endregion

        #region Overrides of System.ValueType

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                ShortGuid guid => _guid.Equals(guid._guid),
                Guid guid => _guid.Equals(guid),
                string _ => _guid.Equals(((ShortGuid) obj)._guid),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        #endregion

        #region Operators

        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            return x._guid == y._guid;
        }

        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }

        public static implicit operator string(ShortGuid shortGuid)
        {
            return shortGuid._value;
        }

        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid._guid;
        }

        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }

        public static implicit operator ShortGuid(Guid guid)
        {
            return new ShortGuid(guid);
        }

        #endregion
    }
}
