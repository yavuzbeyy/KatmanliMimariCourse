using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        public BookService(IGenericRepository<Book> bookRepository,IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<BookCreateDto>> Create(BookCreateDto createBookModel)
        {
            try { 
            if(createBookModel == null)
            {
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Kitap bilgileri boş olamaz."));
            }

            var book = new Book
            {
                Title = createBookModel.Title,
                Description = createBookModel.Description,
                CountOfPage = createBookModel.CountOfPage,
                AuthorId = createBookModel.AuthorId,
                CategoryId = createBookModel.CategoryId,
                RecordDate = DateTime.Now
            };

            _bookRepository.Create(book);

                _logger.LogInformation("Kitap başarıyla oluşturuldu." + book.Title);

                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Success(null, "Kitap başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap oluşturulurken hata oluştu.", createBookModel.Title);
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<BookQueryDto> Delete(int id)
        {
            try { 
            var book = _bookRepository.GetByIdAsync(id).Result;


            if(book == null)
            {
                return ResponseGeneric<BookQueryDto>.Error("Kitap bulunamadı.");
            }

            _bookRepository.Delete(book);
                _logger.LogInformation("Kitap başarıyla silindi.", book.Title);

            return ResponseGeneric<BookQueryDto>.Success(null, "Kitap başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap silinirken hata oluştu.", id);
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<BookQueryDto> GetById(int id)
        {
            try{ 
            var book = _bookRepository.GetByIdAsync(id).Result;

            var bookDto = _mapper.Map<BookQueryDto>(book);

            if (bookDto == null)
            {
            return ResponseGeneric<BookQueryDto>.Success(null, "Kitap bulunamadı.");
            }

            return ResponseGeneric<BookQueryDto>.Success(bookDto, "Kitap başarıyla bulundu.");
            }
            catch (Exception ex)
            {
            return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetByName(string name)
        {
            try {
            var books = _bookRepository.GetAll().Where(x => x.Title == name).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(books);

            if(bookDtos == null || bookDtos.ToList().Count == 0)
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitap döndürüldü.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<BookQueryDto>> ListAll()
        {
            try { 
            var bookList = _bookRepository.GetAll().ToList();

            var bookDtoList = _mapper.Map<IEnumerable<BookQueryDto>>(bookList);

            if (bookDtoList == null || bookDtoList.ToList().Count == 0)
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtoList, "Kitaplar başarıyla döndürüldü.");
            }
            catch (Exception ex)
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetBooksByCategoryId(int categoryId)
        {
            try 
            {
                var booksInCategory = _bookRepository.GetAll().Where(x => x.CategoryId == categoryId).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksInCategory);

                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar başarıyla döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetBooksByAuthorId(int authorId)
        {
            try
            {
                var booksByAuthor = _bookRepository.GetAll().Where(x => x.AuthorId == authorId).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksByAuthor);

                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar başarıyla döndürüldü.");
            }
            catch 
            { 
             return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<BookUpdateDto>> Update(BookUpdateDto bookUpdateDto)
        {
            try 
            {
              //kitabı dbden bul
              var bookEntity = _bookRepository.GetByIdAsync(bookUpdateDto.Id).Result;

                //kitap yoksa hata döndür
                if (bookEntity == null)
                {
                    return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Kitap bulunamadı."));
                }

                //kitap varsa güncelle
                if (!string.IsNullOrEmpty(bookUpdateDto.Title))
                {
                    bookEntity.Title = bookUpdateDto.Title;
                }
                if (!string.IsNullOrEmpty(bookUpdateDto.Description))
                {
                    bookEntity.Description = bookUpdateDto.Description;
                }
                if (bookUpdateDto.CountOfPage != null)
                {
                    bookEntity.CountOfPage = bookUpdateDto.CountOfPage.Value;
                }
                if (bookUpdateDto.AuthorId != null)
                {
                    bookEntity.AuthorId = bookUpdateDto.AuthorId.Value;
                }
                if (bookUpdateDto.CategoryId != null)
                {
                    bookEntity.CategoryId = bookUpdateDto.CategoryId.Value;
                }

                _bookRepository.Update(bookEntity);

                return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Success(null, "Kitap başarıyla güncellendi."));

            }
            catch 
            {
             return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
