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
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IGenericRepository<Book> bookRepository,IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
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

            return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Success(null, "Kitap başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
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

            return ResponseGeneric<BookQueryDto>.Success(null, "Kitap başarıyla silindi.");
            }
            catch (Exception ex)
            {
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

        public Task<IResponse<Book>> Update(Book author)
        {
            throw new NotImplementedException();
        }
    }
}
