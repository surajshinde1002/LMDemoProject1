# educational-resource-repository
Project version accordingly

Project Detailed Project Description:
    Functionalities:
          Resource Upload and Management: Ability for teachers to upload and categorize educational resources.
          Advanced Search: Powerful search capabilities for users to find relevant resources.
          User Access Control: Permissions for who can view or edit resources.
    Technological Requirements:
          Search Engine Optimization: Using Elasticsearch for full-text search and indexing of resources.
          Responsive Frontend: React Virtualized for efficient rendering of large lists of resources.
          Cloud Storage: Azure Blob Storage for storing videos, documents, and other educational materials.


=====================================================================================================

#HNavbar

const url = `${UrlGateway}/gateway/teacher/register`; 

const urlTeacher = `${UrlGateway}/gateway/teacher/login`;

const url = `${UrlGateway}/gateway/user/login`;

========================================================================

# AddResource

const url = `${UrlGateway}/gateway/pdf/Add`;

============================================================================


# Publishable

const url = `${UrlGateway}/gateway/pdf/getPublishablesbystd/${classtd}`; 

const urlResource = `${UrlGateway}/gateway/pdf/Publish/${pdf_Name}/${std}`;

=============================================================================

# Bin

const url = `${UrlGateway}/gateway/pdf/getDeletedbystd/${classtd}`;


const urlResource = `${UrlGateway}/gateway/pdf/Publish/${pdf_Name}/${std}`;


================================================================================

# Resourcelist

const url = `${UrlGateway}/gateway/pdf/getallbystd/${classtd}`;

const urlResource = `${UrlGateway}/gateway/pdf/delete/${pdf_Name}/${id}`;

=======================================================================================


# AddStudent

const url = `${UrlGateway}/gateway/user/register`;

========================================================================================

# ViewStudent

const url = `${UrlGateway}/gateway/user/getActiveStudents/${standard}`;

const url = `${UrlGateway}/gateway/user/delete/${stud_id}`;

const url = `${UrlGateway}/gateway/user/activate/${stud_id}`;


==========================================================================================

# User -> Profile

const url = `${UrlGateway}/gateway/user/byid/${User_id}`;


===========================================================================================

#User -> UHome

const urlResource = `${UrlGateway}/gateway/pdf/getallbystd/${standard}`;


===========================================================================================













