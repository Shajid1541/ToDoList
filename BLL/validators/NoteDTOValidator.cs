using BLL.DTOs;
using DAL;
using FluentValidation;


namespace BLL.validators
{
    public class NoteDTOValidator: AbstractValidator<NoteDTO>
    {
        #region Fields
        private readonly DataAccessFactory dataAccessFactory;
        #endregion

        #region Constructor
        public NoteDTOValidator(DataAccessFactory dataAccessFactory)
        {
            this.dataAccessFactory = dataAccessFactory;

            RuleFor(note => note.NoteTitle)
                .MustAsync(async (_note, title, cancellation) =>
                {
                    var notes = await dataAccessFactory.CreateNoteData().ReadAllAsync();
                    return !notes.Any(note => note.NoteTitle == title && note.userId == _note.UserId);
                }).WithMessage("Note title already exists");
        }
        #endregion

        /*private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var notes = await _noteRepository.ReadAllAsync();
            return !notes.Any(note => note.NoteTitle == title);
        }*/
    }
}
