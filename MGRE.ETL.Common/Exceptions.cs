using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Net.Security;

namespace MGRE.ETL.Common
{
    /// <summary>
    /// Exception class used to raise exceptions from within ETL Service.
    /// Can include exception data inlcuding validation results
    /// </summary> 
    public class MGREException : ApplicationException
    {
        private MGREExceptionData exceptionData;

        public MGREException() { }
        public MGREException(string msg) : base(msg) 
        {
            exceptionData = new MGREExceptionData(msg);
        }

        public MGREException(ValidationResult validation)
            : base(validation.WriteErrors())
        {
            exceptionData = new MGREExceptionData(validation);
        }

        public MGREException(string msg, System.Exception ex) : base(msg, ex) 
        {
            exceptionData = new MGREExceptionData(msg);
        }

        public MGREException(MGREExceptionData exceptionData)
            : base(exceptionData.ErrorString)
        {
            this.exceptionData = exceptionData;
        }

        public MGREExceptionData AppError
        {
            get { return exceptionData; }
        }

    }

    /// <summary>
    /// Class to hold EWatch specific data to include within an EWatchException.Includes Validation result. Written a a DataContract
    /// so that it can be returned from WCF service
    /// </summary>
    [DataContract] 
    public class MGREExceptionData
    {
        [DataMember] 
        private string errorString;

        [DataMember]
        private ValidationResult validationRes;
        
        public MGREExceptionData(string error)
        {
            this.errorString = error;
            validationRes = new ValidationResult();
            validationRes.Errors.Add(new ValidationError(error));
        }

        public MGREExceptionData(ValidationResult validation)
        {
            this.errorString = validation.WriteErrors();
            this.validationRes = validation;
        }

        
        public ValidationResult ValidationRes
        {
            get { return validationRes; }
            set { validationRes = value; }
        }

        
        public string ErrorString
        {
            get { return errorString; }
            set { errorString = value; }
        }

    }

    #region .Net Class Documentation
    /// <summary>
    ///     Class to hold validation errors and warnings from business objects
    ///     Can be used to display errors and warnings to user when calling a business objects validation routine
    ///         or return an error when save being called with invalid data.
    ///     Intended use is BO.Validate returns a ValidationResult object with errors and warnings. The UI would then not call the BO.Save method with invalid
    ///         arguments BUT if it does the BO.Save method would throw a EWatchException with the BO.Validate ValidationResult oject
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="barryc" ref="24063" date="2012-08-10">created</revision>
    /// </history>
    #endregion
    [Serializable]   
    public class ValidationResult
    {

        public List<ValidationWarning> Warnings = new List<ValidationWarning>();
        public List<ValidationError> Errors = new List<ValidationError>();

        public void AddError(int id, string message)
        {
            Errors.Add(new ValidationError(id, message));
        }

        public void AddError( string message)
        {
            AddError(0, message); 
        }

        public void AddWarning(int id, string message)
        {
            Warnings.Add(new ValidationWarning(id, message));
        }

        public void AddWarning(string message)
        {
            AddWarning(0, message); 
        }

        /// <summary>
        /// returns true if any warnings
        /// </summary>
        public bool HasWarnings
        {
            get
            {
                return (Warnings.Count > 0);
            }
        }
        /// <summary>
        ///     Returns true if any errors
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return (Errors.Count > 0);
            }
        }

        /// <summary>
        ///     Writes all errors to s string for exception purposes
        /// </summary>
        /// <returns></returns>
        public string WriteErrors()
        {
            string errors = "";

            foreach (ValidationError error in Errors)
            {
                errors += error.Message + System.Environment.NewLine;
            }

            return errors;
        }

        /// <summary>
        ///     Writes all errors to s string for exception purposes
        /// </summary>
        /// <returns></returns>
        public string WriteWarnings()
        {
            string warnings = "";

            foreach (ValidationWarning warning in Warnings)
            {
                warnings += warning.Message + System.Environment.NewLine;
            }

            return warnings;
        }

        public void Merge(ValidationResult res)
        {
            foreach (ValidationError err in res.Errors)
            {
                Errors.Add(err);
            }

            foreach (ValidationWarning warn in res.Warnings)
            {
                Warnings.Add(warn);
            }
        }
    }


    [Serializable]
    public class ValidationError
    {
        public int ID = 0;
        public string Message = "";

        public ValidationError(int id, string message)
        {
            this.ID = id;
            this.Message = message;
        }

        public ValidationError(string message)
            : this(0, message) 
        {
        }
    }

    [Serializable]
    public class ValidationWarning
    {
        public int ID = 0;
        public string Message = "";

        public ValidationWarning(int id, string message)
        {
            this.ID = id;
            this.Message = message;
        }

        public ValidationWarning(string message)
            : this(0, message)
        {
        }
    }


}
