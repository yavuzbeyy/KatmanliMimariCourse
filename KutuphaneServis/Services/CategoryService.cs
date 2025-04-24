using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Services
{
    public class CategoryService : ICategoryService
    {

        //DI ile GenericRepository'i alıyoruz.
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto categoryCreateDto)
        {
            try { 
            if(categoryCreateDto == null)
            {
                return ResponseGeneric<CategoryCreateDto>.Error("Kategori bilgileri boş olamaz.");
            }

            //DTOyu Entity'e Mapla
            var categoryEntity = new Category { Description = categoryCreateDto.Description, Name = categoryCreateDto.Name };
                categoryEntity.RecordDate = DateTime.Now;

             _categoryRepository.Create(categoryEntity);

            return ResponseGeneric<CategoryCreateDto>.Success(null, "Kategori başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<CategoryCreateDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> Delete(int id)
        {
            try { 
            var category = _categoryRepository.GetByIdAsync(id).Result;

            if(category == null)
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Kategori bulunamadı.");
            }

            _categoryRepository.Delete(category);
            return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori başarıyla silindi.");
                // Delete çağrıldığı anda her koşulda başarılı diye mesaj dönüyoruz ama gerçekten başarılı mı?
            }
            catch (Exception ex)
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> GetById(int id)
        {
            try { 
            var category = _categoryRepository.GetByIdAsync(id).Result;

            var categoryDto = _mapper.Map<CategoryQueryDto>(category);

            if (categoryDto == null)
            {
                return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori bulunamadı.");
            }

            return ResponseGeneric<CategoryQueryDto>.Success(categoryDto, "Kategori başarıyla bulundu.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name)
        {
            try { 
            var categories = _categoryRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

            if (categoryDtos == null || categoryDtos.ToList().Count == 0)
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos, "Kategoriler döndürüldü.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> ListAll()
        {
            try { 
           var categories = _categoryRepository.GetAll().ToList();

                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

            if (categoryDtos == null || categoryDtos.ToList().Count == 0)
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos, "Kategoriler döndürüldü.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<Category>> Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
