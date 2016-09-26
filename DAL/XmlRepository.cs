using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;

using InterfacesLib.Repository;
using InterfacesLib.Shared;


namespace DAL
{
    public class XmlRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> configurations;

        public XmlRepository(string filename)
        {
            FileName = filename;
            if (!File.Exists(filename))
            {
                configurations=new List<T>();
                Update();
            }
            var serializer = new XmlSerializer(typeof(List<T>), null, new Type[] { typeof(T) }, new XmlRootAttribute(typeof(T).Name), "http://ofimbres.wordpress.com/");
      
            using (StreamReader myWriter = new StreamReader(FileName))
            {
                configurations = (List<T>)serializer.Deserialize(myWriter);
                myWriter.Close();
            }
        }

        internal string FileName { get; private set; }

        public T Get(int id)
        {
            T result = null;
            if (configurations != null)
                result = (from c in configurations where c.Id == id select c).FirstOrDefault();
            return result;
        }

        public IQueryable<T> Get()
        {
            return configurations.AsQueryable();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null) 
                return configurations.AsQueryable().Where(predicate);
            return null;
        }

        public T Get(Guid id)
        {
            throw new NotImplementedException();

        }

        public void Add(T entity)
        {
            var id = Get().Count();
            var element = entity;
            while (Get(id)!=null)
            {
                id++;
            }
            element.Id = id;
            configurations.Add(entity);
            Update();
        }

        public void AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(T entity)
        {
            var ent = entity;
            Delete(entity);
            Add(ent);
        }

        public void Update()
        {
            var serializer = new XmlSerializer(configurations.GetType(), null, new Type[] { typeof(T) }, new XmlRootAttribute(typeof(T).Name), "http://ofimbres.wordpress.com/");
            using (var myWriter = new StreamWriter(FileName))
            {
                serializer.Serialize(myWriter, configurations);
                myWriter.Close();
            }
        }

        public void Delete(T entity)
        {
            configurations.Remove(entity);
            Update();
        }

        public void Delete(int id)
        {
            configurations.Remove(configurations.FirstOrDefault(n => n.Id == id));
            Update();
        }

   

  

  
    }
}
