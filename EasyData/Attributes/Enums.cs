using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// Define the possible strategies to set the Primary Key values
    /// </summary>
    [Serializable]
    public enum PrimaryKeyType
    {
        /// <summary>
        /// Use Identity column (auto number)
        /// </summary>
        Identity,
        /// <summary>
        /// Use a sequence
        /// </summary>
        Sequence,
        /// <summary>
        /// Generate a Guid for the primary key
        /// </summary>
        Guid,
        /// <summary>
        /// Use an identity or sequence if supported by the database
        /// </summary>
        Native,
        /// <summary>
        /// The primary key value is always assigned
        /// Note: using this you will lose the ability to call Save(), and will need to call Create() or Update()
        /// explicitly.
        /// </summary>
        Assigned,
        /// <summary>
        /// This is a foreign key to another table
        /// </summary>
        Foreign
    }

    /// <summary>
    /// Define how broken relations should be handled.
    /// </summary>
    [Serializable]
    public enum NotFoundBehaviour
    {
        /// <summary>
        /// Throw an exception when the relation is broken.
        /// </summary>
        Default,

        /// <summary>
        /// Throw an exception when the relation is broken.
        /// </summary>
        /// <remarks>this is the default behaviour</remarks>
        Exception,

        /// <summary>
        /// Ignore the broken relation and update
        /// the FK to null on the next save.
        /// </summary>
        Ignore
    }

    /// <summary>
    /// Define the possible fetch option values
    /// </summary>
    public enum FetchEnum
    {
        /// <summary>
        /// Let EasyData decide what to do here
        /// </summary>
        Unspecified,
        /// <summary>
        /// Use a JOIN to load the data
        /// </summary>
        Join,
        /// <summary>
        /// Use a seperate SELECT statement to load the data
        /// </summary>
        Select,
        /// <summary>
        /// Use a seperate SELECT statement to load the data, re-running the original query in a subselect
        /// </summary>
        SubSelect
    }

    /// <summary>
    /// Defines the cascading behaviour of this association.
    /// </summary>
    /// <remarks>
    /// Entities has associations to other objects, this may be an association to a single item (<see cref="BelongsToAttribute" />)
    /// or an association to a collection (<see cref="HasManyAttribute" />, <see cref="HasManyToAnyAttribute" />).
    /// At any rate, you are able to tell EasyData to automatically traverse an entity's associations, and act according 
    /// to the cascade option. For instance, adding an unsaved entity to a collection with <see cref="CascadeEnum.SaveUpdate" />
    /// cascade will cause it to be saved along with its parent object, without any need for explicit instructions on our side.
    /// </remarks>
    public enum CascadeEnum
    {
        /// <summary>
        /// No cascading. This is the default.
        /// The cascade should be handled manually.
        /// </summary>
        None,
        /// <summary>
        /// Cascade save, update and delete.
        /// When the object is saved, updated or deleted, the associations will be checked
        /// and the objects found will also be saved, updated or deleted.
        /// </summary>
        All,
        /// <summary>
        /// Cascade save and update.
        /// When the object is saved or updated, the associations will be checked and any object that requires
        /// will be saved or updated (including saving or updating the associations in many-to-many scenario).
        /// </summary>
        SaveUpdate,
        /// <summary>
        /// Cascade delete.
        /// When the object is deleted, all the objects in the association will be deleted as well.
        /// </summary>
        Delete
    }

    /// <summary>
    /// Define the relation type for a relation.
    /// </summary>
    [Serializable]
    public enum RelationType
    {
        /// <summary>
        /// Let Active Record guess what is the type of the relation.
        /// </summary>
        Guess,
        /// <summary>
        /// An bag of items (allow duplicates)
        /// </summary>
        Bag,
        /// <summary>
        /// A set of unique items
        /// </summary>
        Set,
        /// <summary>
        /// A bag of items with id
        /// </summary>
        IdBag,
        /// <summary>
        /// Map of key/value pairs (IDictionary)
        /// </summary>
        Map,
        /// <summary>
        /// A list of items - position in the list has meaning
        /// </summary>
        List
    }

    /// <summary>
    /// Defines the cascading behaviour of this association.
    /// </summary>
    /// <remarks>
    /// Entities has associations to other objects, this may be an association to a single item (<see cref="BelongsToAttribute" />)
    /// or an association to a collection (<see cref="HasManyAttribute" />, <see cref="HasManyToAnyAttribute" />).
    /// At any rate, you are able to tell EasyData to automatically traverse an entity's associations, and act according 
    /// to the cascade option. For instance, adding an unsaved entity to a collection with <see cref="CascadeEnum.SaveUpdate" />
    /// cascade will cause it to be saved along with its parent object, without any need for explicit instructions on our side.
    /// </remarks>
    [Serializable]
    public enum ManyRelationCascadeEnum
    {
        /// <summary>
        /// No cascading. This is the default.
        /// The cascade should be handled manually.
        /// </summary>
        None,
        /// <summary>
        /// Cascade save, update and delete.
        /// When the object is saved, updated or deleted, the associations will be checked
        /// and the objects found will also be saved, updated or deleted.
        /// </summary>
        All,
        /// <summary>
        /// Cascade save and update.
        /// When the object is saved or updated, the associations will be checked and any object that requires
        /// will be saved or updated (including saving or updating the associations in many-to-many scenario).
        /// </summary>
        SaveUpdate,
        /// <summary>
        /// Cascade delete.
        /// When the object is deleted, all the objects in the association will be deleted as well.
        /// </summary>
        Delete,
        /// <summary>
        /// Cascade save, update and delete, removing orphan children.
        /// When an object is saved, updated or deleted, the associations will be checked and all objects found
        /// will be saved, updated or deleted as well.
        /// In additional to that, when an object is removed from the association and not associated with another object (orphaned), it will also be deleted.
        /// </summary>
        AllDeleteOrphan
    }

    ///// <summary>
    ///// Nature of the data load
    ///// </summary>
    //[Serializable]
    //public enum PartialLoad
    //{
    //    /// <summary>
    //    /// Load the data when Find method specified as 
    //    /// EasyLoad.Specified <see cref="EasyLoad.Specified" /> or
    //    /// EasyLoad.None <see cref="EasyLoad.None" /> or
    //    /// EasyLoad.Simple <see cref="EasyLoad.Simple" />, 
    //    /// if the property is not from a relation (not a relationship property from another class)
    //    /// </summary>
    //    True,
    //    /// <summary>
    //    /// Default value, load data for the property 
    //    /// when Find method specified as EasyLoad.None <see cref="EasyLoad.None" /> or
    //    /// EasyLoad.Simple <see cref="EasyLoad.Simple" />,
    //    /// if the property is not from a relation (not a relationship property from another class)
    //    /// </summary>
    //    False
    //}
}
