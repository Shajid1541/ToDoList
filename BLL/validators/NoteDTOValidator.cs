using BLL.DTOs;
using DAL;
using DAL.Repositories;
using FluentValidation;


namespace BLL.validators
{
    public class NoteDTOValidator: AbstractValidator<NoteDTO>
    {
        #region Fields
        private readonly NoteRepository noteRepository;
        #endregion

        #region Constructor
        public NoteDTOValidator(NoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;

            RuleFor(note => note.NoteTitle)
                .MustAsync(async (_note, title, cancellation) =>
                {
                    var notes = await noteRepository.ReadAllAsync();
                    return !notes.Any(note => note.NoteTitle == title && note.userId == _note.UserId);
                }).WithMessage("Note title already exists");
        }
        #endregion
    }
}
