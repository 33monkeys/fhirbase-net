# FHIRbase.Net

![fhirbase](https://avatars3.githubusercontent.com/u/6482975?v=3&s=400)

FHIRbase.Net is simple .NET wrappers for FHIRbase (https://github.com/fhirbase).

FHIRbase is an open source relational storage for
[FHIR](http://hl7.org/implement/standards/fhir/) targeting real production.


To install Simple .NET wrappers for FHIRbase, run the following command in the Package Manager Console:

	PM> Install-Package Fhirbase.Net -Pre


Example:

	var patient = new Patient();
        patient.Name.Add(HumanName.ForFamily("Hello").WithGiven("World"));
        patient.Telecom.Add(new ContactPoint
        {
                System = ContactPoint.ContactPointSystem.Phone, 
                Use = ContactPoint.ContactPointUse.Mobile,
                Value = "123456789"
        });

        var createdPatient = (Patient)FHIRbase.Create(patient);

	...

	FHIRbase.Delete(createdPatient);
