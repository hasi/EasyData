using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyData.Query
{
	public interface IQuery
	{
        //bool Execute();

        bool Update();

        bool Delete();

        //IList Select();

        //IQuery From(Type type);

        //IQuery Where();

        //IQuery Join();

        //IQuery Order();

        //IQuery Group();

        //IQuery Top(int size);

        //IQuery Like();

        //IQuery Between();

        //IQuery Distinct();

		/// <summary>
		/// Bind a value to an indexed parameter.
		/// </summary>
		/// <param name="position">Position of the parameter in the query, numbered from <c>0</c></param>
		/// <param name="val">The possibly null parameter value</param>
		/// <param name="type">The Hibernate type</param>
        //IQuery SetParameter(int position, object val, Type type);

		/// <summary>
		/// Bind a value to a named query parameter
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">The possibly null parameter value</param>
		/// <param name="type">The NHibernate <see cref="IType"/>.</param>
		IQuery SetParameter(string name, object val, Type type);

		/// <summary>
		/// Bind a value to an indexed parameter.
		/// </summary>
		/// <param name="position">Position of the parameter in the query, numbered from <c>0</c></param>
		/// <param name="val">The possibly null parameter value</param>
		/// <typeparam name="T">The parameter's <see cref="Type"/> </typeparam>
        //IQuery SetParameter<T>(int position, T val);

		/// <summary>
		/// Bind a value to a named query parameter
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">The possibly null parameter value</param>
		/// <typeparam name="T">The parameter's <see cref="Type"/> </typeparam>
		IQuery SetParameter<T>(string name, T val);

		/// <summary>
		/// Bind multiple values to a named query parameter. This is useful for binding a list
		/// of values to an expression such as <c>foo.bar in (:value_list)</c>
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="vals">A collection of values to list</param>
		/// <param name="type">The Hibernate type of the values</param>
		IQuery SetParameterList(string name, ICollection vals, Type type);

		/// <summary> 
		/// Bind multiple values to a named query parameter. This is useful for binding
		/// a list of values to an expression such as <tt>foo.bar in (:value_list)</tt>.
		/// </summary>
		/// <param name="name">the name of the parameter </param>
		/// <param name="vals">a collection of values to list </param>
		/// <param name="type">the Hibernate type of the values </param>
		IQuery SetParameterList(string name, object[] vals, Type type);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> array to an indexed parameter
		/// using an NHibernate <see cref="BinaryType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/> array.</param>
        //IQuery SetBinary(int position, byte[] val);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> array to a named parameter
		/// using an NHibernate <see cref="BinaryType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/> array.</param>
		IQuery SetBinary(string name, byte[] val);

		/// <summary>
		/// Bind an instance of a <see cref="Boolean" /> to an indexed parameter
		/// using an NHibernate <see cref="BooleanType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Boolean"/>.</param>
        //IQuery SetBoolean(int position, bool val);

		/// <summary>
		/// Bind an instance of a <see cref="Boolean" /> to a named parameter
		/// using an NHibernate <see cref="BooleanType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Boolean"/>.</param>
		IQuery SetBoolean(string name, bool val);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> to an indexed parameter
		/// using an NHibernate <see cref="ByteType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/>.</param>
        //IQuery SetByte(int position, byte val);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> to a named parameter
		/// using an NHibernate <see cref="ByteType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/>.</param>
		IQuery SetByte(string name, byte val);

		/// <summary>
		/// Bind an instance of a <see cref="Char" /> to an indexed parameter
		/// using an NHibernate <see cref="CharType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Char"/>.</param>
        //IQuery SetCharacter(int position, char val);

		/// <summary>
		/// Bind an instance of a <see cref="Char" /> to a named parameter
		/// using an NHibernate <see cref="CharType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Char"/>.</param>
		IQuery SetCharacter(string name, char val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to an indexed parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
        //IQuery SetDateTime(int position, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		/// <param name="name">The name of the parameter</param>
		IQuery SetDateTime(string name, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="Decimal" /> to an indexed parameter
		/// using an NHibernate <see cref="DecimalType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Decimal"/>.</param>
        //IQuery SetDecimal(int position, decimal val);

		/// <summary>
		/// Bind an instance of a <see cref="Decimal" /> to a named parameter
		/// using an NHibernate <see cref="DecimalType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Decimal"/>.</param>
		IQuery SetDecimal(string name, decimal val);

		/// <summary>
		/// Bind an instance of a <see cref="Double" /> to an indexed parameter
		/// using an NHibernate <see cref="DoubleType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Double"/>.</param>
        //IQuery SetDouble(int position, double val);

		/// <summary>
		/// Bind an instance of a <see cref="Double" /> to a named parameter
		/// using an NHibernate <see cref="DoubleType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Double"/>.</param>
		IQuery SetDouble(string name, double val);

		/// <summary>
		/// Bind an instance of a persistent enumeration class to an indexed parameter
		/// using an NHibernate <see cref="PersistentEnumType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a persistent enumeration</param>
        //IQuery SetEnum(int position, Enum val);

		/// <summary>
		/// Bind an instance of a persistent enumeration class to a named parameter
		/// using an NHibernate <see cref="PersistentEnumType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a persistent enumeration</param>
		IQuery SetEnum(string name, Enum val);

		/// <summary>
		/// Bind an instance of a <see cref="Int16" /> to an indexed parameter
		/// using an NHibernate <see cref="Int16Type"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Int16"/>.</param>
        //IQuery SetInt16(int position, short val);

		/// <summary>
		/// Bind an instance of a <see cref="Int16" /> to a named parameter
		/// using an NHibernate <see cref="Int16Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int16"/>.</param>
		IQuery SetInt16(string name, short val);

		/// <summary>
		/// Bind an instance of a <see cref="Int32" /> to an indexed parameter
		/// using an NHibernate <see cref="Int32Type"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Int32"/>.</param>
        //IQuery SetInt32(int position, int val);

		/// <summary>
		/// Bind an instance of a <see cref="Int32" /> to a named parameter
		/// using an NHibernate <see cref="Int32Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int32"/>.</param>
		IQuery SetInt32(string name, int val);

		/// <summary>
		/// Bind an instance of a <see cref="Int64" /> to an indexed parameter
		/// using an NHibernate <see cref="Int64Type"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="Int64"/>.</param>
        //IQuery SetInt64(int position, long val);

		/// <summary>
		/// Bind an instance of a <see cref="Int64" /> to a named parameter
		/// using an NHibernate <see cref="Int64Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int64"/>.</param>
		IQuery SetInt64(string name, long val);

		/// <summary>
		/// Bind an instance of a <see cref="String" /> to an indexed parameter
		/// using an NHibernate <see cref="StringType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="String"/>.</param>
        //IQuery SetString(int position, string val);

		/// <summary>
		/// Bind an instance of a <see cref="String" /> to a named parameter
		/// using an NHibernate <see cref="StringType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="String"/>.</param>
		IQuery SetString(string name, string val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to an indexed parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
        //IQuery SetTime(int position, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		IQuery SetTime(string name, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to an indexed parameter
		/// using an NHibernate <see cref="TimestampType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
        //IQuery SetTimestamp(int position, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="TimestampType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		IQuery SetTimestamp(string name, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="Guid" /> to a named parameter
		/// using an NHibernate <see cref="GuidType"/>.
		/// </summary>
		/// <param name="position">The position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name="val">An instance of a <see cref="Guid"/>.</param>
        //IQuery SetGuid(int position, Guid val);

		/// <summary>
		/// Bind an instance of a <see cref="Guid" /> to a named parameter
		/// using an NHibernate <see cref="GuidType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">An instance of a <see cref="Guid"/>.</param>
		IQuery SetGuid(string name, Guid val);
	}
}
