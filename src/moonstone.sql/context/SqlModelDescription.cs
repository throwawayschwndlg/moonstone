using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.context
{
    public abstract class SqlModelDescription { }

    public class SqlModelDescription<T> : SqlModelDescription
    {
        public string Table { get; set; }

        public Type Type { get; set; }

        protected ICollection<SqlPropertyDescription> PropertyDescriptions { get; set; }

        public SqlModelDescription(Type type, string table)
        {
            this.Type = type;
            this.Table = table;
            this.PropertyDescriptions = new List<SqlPropertyDescription>();
        }

        public static SqlModelDescription<T> Auto(string table)
        {
            var ignore_insert_fields = new string[] { "id" };
            var ignore_update_fields = new string[] { "id" };

            var type = typeof(T);
            var properties = type.GetProperties();

            var modelDescription = new SqlModelDescription<T>(type, table);

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    // Ignore read-only properties
                    continue;
                }

                bool ignoreOnInsert = ignore_insert_fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase);

                bool ignoreOnUpdate = ignore_update_fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase);

                var propertyDescription = new SqlPropertyDescription(
                    propertyName: property.Name,
                    fieldName: property.Name.ToLower(),
                    ignoreOnInsert: ignoreOnInsert,
                    ignoreOnUpdate: ignoreOnUpdate);

                modelDescription.RegisterProperty(propertyDescription: propertyDescription);
            }

            return modelDescription;
        }

        public IEnumerable<SqlPropertyDescription> Properties()
        {
            return this.PropertyDescriptions;
        }

        public SqlPropertyDescription Property<TProperty>(Expression<Func<T, TProperty>> prop)
        {
            var memberExpression = prop.Body as MemberExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            return this.Property(propertyInfo.Name);
        }

        public void RegisterProperty(SqlPropertyDescription propertyDescription)
        {
            this.PropertyDescriptions.Add(propertyDescription);
        }

        protected SqlPropertyDescription Property(string propertyName)
        {
            return this.PropertyDescriptions
                .FirstOrDefault(p =>
                    p.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class SqlPropertyDescription
    {
        /// <summary>
        /// Name of the property
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Controls if the property will be used in insert statements
        /// </summary>
        public bool IgnoreOnInsert { get; set; }

        /// <summary>
        /// Controls if the property will be used in update statements
        /// </summary>
        public bool IgnoreOnUpdate { get; set; }

        /// <summary>
        /// Name of the database table field
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Initializes a new SqlPropertyDescription
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="fieldName">Name of the database table field</param>
        public SqlPropertyDescription(string propertyName, string fieldName, bool ignoreOnInsert, bool ignoreOnUpdate)
        {
            this.PropertyName = propertyName;
            this.FieldName = fieldName;
            this.IgnoreOnInsert = ignoreOnInsert;
            this.IgnoreOnUpdate = ignoreOnUpdate;
        }
    }
}