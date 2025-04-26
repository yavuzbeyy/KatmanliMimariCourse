using AutoMapper;
using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
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
    public class AuthorService : IAuthorService
    {
        //DI ile GenericRepository'i alıyoruz.

        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IGenericRepository<Author> authorRepository,IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<IResponse<Author>> Create(AuthorCreateDto authorCreateDto)
        {
            try 
            {
                if (authorCreateDto == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
                }

                var newAuthor = _mapper.Map<Author>(authorCreateDto); // AutoMapper ile DTO'yu Entity'e dönüştürüyoruz.
                newAuthor.RecordDate = DateTime.Now;              

                _authorRepository.Create(newAuthor);

                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(null, "Yazar başarıyla oluşturuldu."));
            }catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Bir hata oluştu."));
            }

        }

        public IResponse<Author> Delete(int id)
        {
            try 
            {
                //Önce Entity varmı onu bul
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
                }
                //Entity varsa sil
                _authorRepository.Delete(author);
                return ResponseGeneric<Author>.Success(null,"Yazar başarıyla silindi.");
            }
            catch {
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
                   }

        }

        public IResponse<AuthorQueryDto> GetById(int id)
        {
            try 
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                var authorQueryDto = _mapper.Map<AuthorQueryDto>(author); // AutoMapper ile Entity'i DTO'ya dönüştürüyoruz.

                if (author == null)
                {
                    return ResponseGeneric<AuthorQueryDto>.Success(null, "Yazar bulunamadı.");
                }
                return ResponseGeneric<AuthorQueryDto>.Success(authorQueryDto, "Yazar başarıyla bulundu.");
            }catch
            {
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name)
        {
            try 
            {
                var authorList = _authorRepository.GetAll().Where(x => x.Name == name).ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(authorList);

                if (authorQueryDtos == null || authorQueryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<AuthorQueryDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(allAuthors);

                if (allAuthors == null || allAuthors.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazarlar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazarlar listelendi");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }


        }

        public Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorUpdateDto)
        {

            try { 
            var authorEntity = _authorRepository.GetByIdAsync(authorUpdateDto.Id).Result;

            if (authorEntity == null)
            {
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Yazar bulunamadı."));
            }

            if (authorUpdateDto.Name != null)
            {
                authorEntity.Name = authorUpdateDto.Name;
            }
            if (authorUpdateDto.Surname != null)
            {
                authorEntity.Surname = authorUpdateDto.Surname;
            }
            if (authorUpdateDto.PlaceOfBirth != null)
            {
                authorEntity.PlaceOfBirth = authorUpdateDto.PlaceOfBirth;
            }
            if (authorUpdateDto.YearOfBirth != null)
            {
                authorEntity.YearOfBirth = authorUpdateDto.YearOfBirth ?? authorEntity.YearOfBirth;
            }

            _authorRepository.Update(authorEntity);

            return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Success(null, "Yazar başarıyla güncellendi."));
            }
            catch 
            { 
            return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Yazar güncellenirken hata oluştu."));
            }


        }
    }
}
