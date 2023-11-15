namespace EngineBay.DemoModule
{
    using FluentValidation;

    public class UpdateTodoListDtoValidator : AbstractValidator<UpdateTodoListCommand>
    {
        public UpdateTodoListDtoValidator()
        {
            this.RuleFor(updateTodoListCommand => updateTodoListCommand.Id).NotNull().NotEmpty();
            this.RuleFor(updateTodoListCommand => updateTodoListCommand.Name).NotNull().NotEmpty();
        }
    }
}