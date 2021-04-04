Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity.Validation
Imports System.Text

Public Class EntityEnhancedExceptionFactory
    Public Shared Function GetEnhancedDbUpdateException(updateEx As DbUpdateException) As Exception
        Dim builder = New StringBuilder()
        builder.AppendLine($"Details: {updateEx?.InnerException?.InnerException.Message}. ")
        For Each dbEntityEntry In (If(IsNothing(updateEx?.Entries), New List(Of DbEntityEntry)(), updateEx?.Entries))
            builder.AppendLine($"Entity of type {dbEntityEntry.Entity.GetType().Name} in state {dbEntityEntry.State} could not be updated.")
        Next
        Return New Exception(builder.ToString(), updateEx)
    End Function

    Public Shared Function GetEnhancedDbEntityValidationException(validationEx As DbEntityValidationException) As Exception
        Dim builder = New StringBuilder()
        For Each outerError In validationEx.EntityValidationErrors
            builder.AppendLine($"Entity of type {outerError.Entry.Entity.GetType().Name} in state {outerError.Entry.State} could not be updated.")
            For Each validationError In outerError.ValidationErrors
                builder.AppendLine($"- Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}")
            Next
        Next
        Return New Exception(builder.ToString(), validationEx)
    End Function

End Class
