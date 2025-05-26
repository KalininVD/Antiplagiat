# Antiplagiat Service (Backend)

## Service overview

**Antiplagiat** is a backend system built with a microservice architecture, consisting of three main services:

- **File Storing Service** $-$ a service for storing and retrieving text files (.txt) from users and checking for duplicate content within these files.
- **File Analysis Service** $-$ a service for analyzing uploaded text files (calculating some statistics and generating WordCloud images)
- **API Gateway** $-$ a single entry point for interacting with the services, potentially including additional features such as authentication and rate limiting

Both main services (File Storing Service and File Analysis Service) have OpenAPI documentation, available within Swagger UI, which is also proxied and available through the API Gateway. For testing purposes, there is a test text file available in the repository (`Lorem ipsum.txt`), which can be used to test the functionality of the services $-$ load it to the File Storing Service and then retrieve the analytical report and WordCloud image for it from the File Analysis Service.

## File Storing Service

The File Storing Service is designed using a layered architecture, including:

- **FileStoringService.Domain**: Contains core business logic and domain entities related to file management: `File` entity and a hashing service for detecting duplicate content without comparing file contents directly.
- **FileStoringService.Application**: Implements application-specific logic, such as file saving and retrieval operations. Also contains DTOs (Data Transfer Objects) for transferring data between layers and services.
- **FileStoringService.Infrastructure**: Responsible for the actual storage of files saving the data about loaded files in a PostgreSQL database. The main table `File` maps file identifiers to their content hashes, allowing for efficient duplicate detection without loading file content from the storage. Storage Provider on the current stage is represented by a simple file saver that saves files to the local file system (specified directory inside the Docker container if running in Docker) with adding timestamps to file names to avoid problems with saving files with the same name. In the future, it can be extended to support cloud storage providers like AWS S3 or Azure Blob Storage.
- **FileStoringService.Web**: Exposes the service functionality via a RESTful Web API (see Web API Endpoints section below). It also includes Swagger UI for API documentation and testing.

### Web API Endpoints

- **POST /files** $-$ Accepts a .txt file for upload. If a file with the same content already exists, the response contains the identifier of the existing file and a flag plagiat: true. Otherwise, the file is saved, and its new identifier is returned with plagiat: false.
- **GET /files/{id}** $-$ Retrieves the file stored with the specified identifier. If the file does not exist, a `404 Not Found` response is returned.

If there is an error during file upload, the service returns a `500 Internal Server Error` response or `400 Bad Request` if the file cannot be correctly processed (e.g., not a .txt file).

## File Analysis Service

The File Analysis Service is also designed using the same layered architecture as the File Storing Service:

- **FileAnalysisService.Domain**: Contains the core business logic and domain entities for file analysis: `AnaliticalReport` and `WordCloud` entities, which represent the results of file analysis and generated WordCloud images, respectively. It also includes a service for calculating statistics (`FileStatistics`) based on file contents such as word count, character count, and paragraph count.
- **FileAnalysisService.Application**: Implements application-specific logic, such as retrieving file content from the File Storing Service and generating WordCloud images using a third-party API. It also contains DTOs for transferring data between layers and services. Both `ReportService` and `WordCloudService` implement logic for generating analytical reports and WordCloud images, respectively, if they are not already available (created earlier).
- **FileAnalysisService.Infrastructure**: Responsible for the storage of analytical reports in a PostgreSQL database. Since the identifier for the report is the same as the identifier of the file in the File Storing Service, the same request will use already existing report if it was created earlier. Additionally, since generated WordCloud images are stored in the specified directory, the full path to the image is stored in the database, allowing for easy retrieval without needing to generate the image again.
- **FileAnalysisService.Web**: Exposes the service functionality via a RESTful Web API and includes Swagger UI for API documentation and testing.

### Web API Endpoints

- **GET /reports/{id}** $-$ Retrieves the analytical report for the file with the specified identifier. If the report does not exist, it is firstly generated from the file content retrieved from the File Storing Service, and then returned. If the file does not exist (in File Storing Service), a `404 Not Found` response is returned.
- **GET /reports/{id}/wordcloud** $-$ Retrieves the WordCloud image for the file with the specified identifier. If the WordCloud image does not exist, it is generated from the file content retrieved from the File Storing Service, and then returned as .png file. If the file with the specified identifier was not found in the File Storing Service, a `404 Not Found` response is returned.

## API Gateway

The API Gateway serves as a single entry point for interacting with the File Storing Service and File Analysis Service. It is responsible for routing requests to the appropriate service (acting as a simple reverse proxy). Since all requests to the API Gateway are just routed to the corresponding service, it does not have its own business logic and if one of the services is unavailable or returns an error, the API Gateway will return the same error response to the client. In the future, the API Gateway can be extended to include additional features such as authentication, rate limiting, and logging.

## Overall frontend ideas applicable to the existing backend

Since the plagiarism detection is handled on the very first step (uploading the file to the File Storing Service), the frontend can be designed to provide a user-friendly interface for uploading files and getting a warning if the file matches an already loaded file. After the file is uploaded, the frontend can display the analytical report and WordCloud image for the file, allowing users to visualize the content and statistics of their text files. In the future, the frontend can also be extended to include features such as user authentication, file management (deleting files, viewing file history), and more advanced analytics, but these features then will require additional backend functionality to be implemented in the File Storing Service or File Analysis Service (e.g., calculating the actual similarity between files or the percentage of plagiarism, removing files to not include them in the analysis for the rest of the users, etc.).

## Running the services locally with Docker

To run the Antiplagiat services locally using Docker, you need to have Docker installed on your machine. The services are configured to run in Docker containers, and you can use Docker Compose to manage them:

```bash
docker compose -f "docker-compose.yml" up -d --build
```

But for correct start of the services, you need to create a `.env` file in the root directory (where `docker-compose.yml` is located) with all the necessary environment variables. The example `.env.example` file is provided in the repository, and you can copy/rename it to `.env` and modify the required values.

**Note**: The `ASPNETCORE_ENVIRONMENT` variable in the `.env` file should be set to `Development` only for local development/testing, but other values are not currently supported since all the required configurations are set up in `appsettings.Development.json` files for all services.

After the services are up and running, you can access the Swagger UI for each service at the following URLs:

- File Storing Service: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- File Analysis Service: [http://localhost:5002/swagger](http://localhost:5002/swagger)

API Gateway does not have its own Swagger UI, but you can access both File Storing Service and File Analysis Service Swagger UIs through it at the following URL: [http://localhost:8080/swagger/filestoring](http://localhost:8080/swagger/filestoring) and [http://localhost:8080/swagger/fileanalysis](http://localhost:8080/swagger/fileanalysis).

**Note**: The ports used in the example above are the default ones specified in the `.env.example` file. If you change the ports in your `.env` file, make sure to access the Swagger UI at the correct URLs. You can also change the names of the services in the `docker-compose.yml` file if you want to access their Swagger UIs through the API Gateway with different paths.

For accessing the pure services, you can use `curl` commands or any HTTP client like Postman (or even just a browser) to interact with the services directly, using the following endpoints:

- File Storing Service:
  - Upload file: `POST http://localhost:5001/files`
  - Retrieve file: `GET http://localhost:5001/files/{id}`

- File Analysis Service:
  - Retrieve analytical report: `GET http://localhost:5002/reports/{id}`
  - Retrieve WordCloud image: `GET http://localhost:5002/reports/{id}/wordcloud`

Obviously, you need to make sure the same ports are used as specified in your `.env` file, or replace both ports with API Gateway port to access both services with the same base URL. `{id}` in the URLs should be replaced with the actual file identifier returned by the File Storing Service when you upload a file, otherwise you will get a `404 Not Found` response or `400 Bad Request` if the passed identifier is not a valid GUID.