using BLL.DTOs;
using DAL;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.validators
{
    public class NoteDTOValidator: AbstractValidator<NoteDTO>
    {
        private readonly DataAccessFactory dataAccessFactory;

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

        /*private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var notes = await _noteRepository.ReadAllAsync();
            return !notes.Any(note => note.NoteTitle == title);
        }*/
    }
}
