using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToDos.MinimalAPI.ToDO
{
    public static class ToDoRequest
    {
        public static WebApplication RegisterEndPoints(this WebApplication app)
        {
            app.MapGet("/todos", ToDoRequest.GetAll)
                .Produces<List<ToDo>>()
                .WithTags("To Dos");

            app.MapGet("/todos/{id}", ToDoRequest.GetById)
                .Produces<ToDo>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("To Dos");

            app.MapPost("/todos", ToDoRequest.Create)
                .Produces<ToDo>(StatusCodes.Status201Created)
                .Accepts<ToDo>("application/json")
                .WithTags("To Dos")
                .WithValidator<ToDo>();

            app.MapPut("/todos/{id}", ToDoRequest.Update)
                .WithValidator<ToDo>()
                .Produces<ToDo>(StatusCodes.Status204NoContent)
                .Produces<ToDo>(StatusCodes.Status404NotFound)
                .Accepts<ToDo>("application/json")
                .WithTags("To Dos");

            app.MapDelete("/todos/{id}", ToDoRequest.Delete)
                .Produces<ToDo>(StatusCodes.Status204NoContent)
                .Produces<ToDo>(StatusCodes.Status404NotFound)
                .WithTags("To Dos")
                .ExcludeFromDescription();

            return app;
        }

        public static IResult GetAll(IToDoService service)
        {
            var todos = service.GetAll();
            return Results.Ok(todos);
        }

        public static IResult GetById(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);
            if(todo == null)
            {
                return Results.NotFound(); //404
            }

            return Results.Ok(todo); //200
        }

        public static IResult Create(IToDoService service, ToDo toDo)
        {
            //var validationResult = validator.Validate(toDo);
            //if (!validationResult.IsValid)
            //{
            //    return Results.BadRequest(validationResult.Errors);
            //}

            service.Create(toDo);

            return Results.Created($"/todos/{toDo.Id}", toDo);
        }

        public static IResult Update(IToDoService service, Guid id, ToDo toDo)
        {
            //var validationResult = validator.Validate(toDo);
            //if (!validationResult.IsValid)
            //{
            //    return Results.BadRequest(validationResult.Errors);
            //}

            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound(); //404
            }

            service.Update(todo);

            return Results.NoContent(); //204
        }

        public static IResult Delete(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound(); //404
            }

            service.Delete(id);
            return Results.NoContent(); //204
        }
    }
}
