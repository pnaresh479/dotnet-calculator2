pipeline {
    agent any

    environment {
        SONARQUBE_SERVER = 'sonarcloud' // Name configured in Jenkins for SonarQube
        PROJECT_KEY = 'sonarcloud-token-jenkins' // Replace with your SonarCloud project key
        ORGANIZATION = 'pnaresh479' // Replace with your SonarCloud organization
    }

    stages {
        stage('Checkout Code') {
            steps {
                echo 'Checking out code from GitHub...'
                git branch: 'main', url: 'https://github.com/pnaresh479/dotnet-calculator2.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo 'Restoring .NET dependencies...'
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                echo 'Building the project...'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Prepare Installer') {
            steps {
                echo 'Preparing the installer...'
                // Replace this with the actual command to create an installer (e.g., WiX, Inno Setup, etc.)
                bat 'dotnet publish -c Release -o output'
            }
        }

        stage('SonarQube Analysis') {
            steps {
                echo 'Running SonarQube analysis...'
                withSonarQubeEnv(SONARQUBE_SERVER) {
                    bat """
                    dotnet sonarscanner begin /k:"${PROJECT_KEY}" /o:"${ORGANIZATION}" /d:sonar.host.url=https://sonarcloud.io
                    dotnet build
                    dotnet sonarscanner end
                    """
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}