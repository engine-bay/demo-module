namespace EngineBay.DemoModule
{
    using FluentValidation;

    public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemCommand>
    {
        public UpdateTodoItemDtoValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.ListId).NotNull().NotEmpty();
        }
    }
}