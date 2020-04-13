using System.Collections.Generic;

namespace Foodsharing_app.Services
{
    public interface ICrudService<T>
    {
        public List<T> Get();

        public T Get(string id);

        public T Create(T user);

        public void Update(string id, T item);

        public void Remove(T item);

        public void Remove(string id);
    }
}