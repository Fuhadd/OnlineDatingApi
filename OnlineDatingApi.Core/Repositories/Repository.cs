using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineDatingApi.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        
        private readonly DbSet<T> _db;
        byte[]? imageBytes;
       
        public Repository(DatabaseContext context)
        {
            _context = context;
           
            _db = _context.Set<T>();
        }

        
        public async Task Create(T entity)
        {
            
            await _db.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            if (entity != null)
            {
                _db.Remove(entity);
            }
        }

        

        public async Task<T?> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _db.AsQueryable();

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            var result = await query.AsNoTracking().FirstOrDefaultAsync(expression);
            
            if (result != null)
            {
                return result;
            }

            return null; 
        }

        

        public async Task<IList<T>> GetAll(
                              Expression<Func<T, bool>>? expression = null, 
                              Func<IQueryable<T>, 
                              IOrderedQueryable<T>>? orderBy = null, 
                              List<string>? includes = null,
                              int limit = 0)
        {
            IQueryable<T> query = _db;


            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            var result = await query.AsNoTracking().ToListAsync();
            
            return result;


        }

        //public async Task<string?> UploadImage(List<IFormFile> images)
        //{

            
        //       if (images.Count == 0)
        //            return null;

        //        string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Images");
        //        if (!Directory.Exists(directoryPath))
        //            Directory.CreateDirectory(directoryPath);

        //        foreach (var image in images)
        //        {
        //            long imageSize;
        //            string imagePath = Path.Combine(directoryPath, image.FileName);
        //            using (var stream = new FileStream(imagePath, FileMode.Create))
        //            {

        //                await image.CopyToAsync(stream);
        //                imageSize = stream.Length;


        //            }


        //            CreateImagesUrlDTO userImage = new CreateImagesUrlDTO();
        //            userImage.Id = Guid.NewGuid();
        //            userImage.ImageUrl = imagePath;
        //            userImage.Size = imageSize;
        //            userImage.ApiUserId = "5429120f-3682-47ce-aa1d-4fc9351e150c";

        //            primes.Add(userImage);
        //        }

        //        return Ok(primes);


        //    }
        //}

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
 
        }

        void IRepository<T>.SaveImageBinary(string localImage)
        {
            throw new NotImplementedException();
        }
    }
}
