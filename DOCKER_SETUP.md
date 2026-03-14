# Demo Shop - Local Development Setup

This docker-compose file sets up the required infrastructure services for local development.

## Services

### RabbitMQ
- **AMQP Port**: 5672
- **Management UI**: http://localhost:15672
- **Username**: demo
- **Password**: demo123

### MinIO (S3-Compatible Object Storage)
- **API Port**: 9000
- **Console UI**: http://localhost:9001
- **Username**: minioadmin
- **Password**: minioadmin

## Getting Started

### 1. Start Services
```bash
docker-compose up -d
```

### 2. Access MinIO Console
1. Open http://localhost:9001
2. Login with `minioadmin` / `minioadmin`
3. Create a bucket named `invoices`

### 3. Configure Your Application
The InvoiceService is already configured to use MinIO in Development mode.
See `InvoiceService.Api/appsettings.Development.json`

### 4. Stop Services
```bash
docker-compose down
```

### 5. Reset All Data
```bash
docker-compose down -v
```

## Verification

After starting the services:

- **RabbitMQ**: Visit http://localhost:15672 and login
- **MinIO**: Visit http://localhost:9001 and login
- **Check running containers**: `docker ps`

## Creating the MinIO Bucket

You have two options:

### Option A: Via Web UI
1. Go to http://localhost:9001
2. Login with `minioadmin` / `minioadmin`
3. Click "Create Bucket"
4. Name it `invoices`
5. Click "Create Bucket"

### Option B: Via MinIO Client (mc)
```bash
# Install MinIO client
docker run --rm -it --network demo-shop-network minio/mc alias set myminio http://minio:9000 minioadmin minioadmin

# Create bucket
docker run --rm -it --network demo-shop-network minio/mc mb myminio/invoices
```

## Network

All services are on the `demo-shop-network` bridge network, so they can communicate with each other using their service names (e.g., `rabbitmq`, `minio`).

## Volumes

Data is persisted in Docker volumes:
- `rabbitmq_data` - RabbitMQ messages and configuration
- `minio_data` - MinIO object storage data

To inspect volumes:
```bash
docker volume ls
docker volume inspect demo-shop_minio_data
```
