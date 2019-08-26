# Infrastructure

The infrastrcuture is build using Terraform you can find the infrastructure definitions in the infrastruture folder in this repository.  Before begenning you will need to setup the state store.  This is setup to use and Azure Storage account.  You will need to create an account and container with the same details as those defined in the `main.tf` backend configuration.  You will also need to set the `ARM_ACCESS_KEY` environment variable to an access key for the storage account. You set this with the command `$Env:ARM_ACCESS_KEY +=`

To execute the infrastrucutre:
1. Run az login from the command line and follow the instructures you will need access to the subscriptions that are used.
1. Run `terraform apply -var 'env=dev' -var 'sqlAdminPassword={add-password-here}'` (you need to define the name of the environment you want to create)


### Todo 

1. State storage 
1. Talk about naming conventions