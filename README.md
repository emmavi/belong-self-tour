# Belong Self Tour technical exam

[Project Requirement](https://github.com/emmavi/belong-self-tour/Backend_Engineer_Onsite_Project_II.pdf "Project Requirement")

## Project structure

- Belong.SelfTours.Domain
- Belong.SelfTours.Infra
- Belong.SelfTours.SelfToursAPI

### **Belong.SelfTours.Domain**

Everything related to the Domain entities and contracts for the persistence

### **Belong.SelfTours.Infra**

Implementation of the Domain's repositories's contracts.
The Proxy to get the Home info it's inside the HomeRepository because it doesn't matter how do you populate the info or where it's persisted.

### **Belong.SelfTours.SelfToursAPI**

Endpoints and business logic for the Domain

\
&nbsp;

---

## Pending items

- Move the business logic to a Service Project or inside the Domain as if it was DDD so it won't be attached to the API project.
- Implement logging with NLog
- Implement IGenericRepository and GenericRepository
- Implement pending repositories methods
