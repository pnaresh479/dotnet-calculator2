pipeline {
    agent {
        label 'windows'   // Ensure it runs on a Windows agent
    }

    environment {
        SONARQUBE_SERVER = 'sonarcloud' // Name configured in Jenkins
        PROJECT_KEY = 'dotnet-calculator2' // Replace with your actual SonarCloud project key
        ORGANIZATION = 'pnaresh479' // Replace with your SonarCloud organization
        BUILD_CONFIG = 'Release'
        APP_PATH = 'CalculatorApp'
        INSTALLER_PATH = 'Installer'
        OUTPUT_DIR = 'output'
    }

    stages {

        stage('Checkout Code') {
            steps {
                echo '📦 Checking out code from GitHub...'
                git branch: 'main', url: 'https://github.com/pnaresh479/dotnet-calculator2.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo '🔧 Restoring .NET dependencies...'
                dir("${APP_PATH}") {
                    bat "dotnet restore"
                }
            }
        }

        stage('Build Application') {
            steps {
                echo '🏗️ Building the .NET project...'
                dir("${APP_PATH}") {
                    bat "dotnet build --configuration ${BUILD_CONFIG}"
                }
            }
        }

        stage('Publish Application') {
            steps {
                echo '🚀 Publishing application for installer...'
                dir("${APP_PATH}") {
                    bat "dotnet publish -c ${BUILD_CONFIG} -o ../${OUTPUT_DIR}"
                }
            }
        }

        stage('Run SonarQube Analysis') {
            steps {
                echo '🔍 Running SonarCloud analysis...'
                withSonarQubeEnv("${SONARQUBE_SERVER}") {
                    dir("${APP_PATH}") {
                        bat """
                            dotnet sonarscanner begin /k:"${PROJECT_KEY}" /o:"${ORGANIZATION}" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login=%SONAR_AUTH_TOKEN%
                            dotnet build --configuration ${BUILD_CONFIG}
                            dotnet sonarscanner end /d:sonar.login=%SONAR_AUTH_TOKEN%
                        """
                    }
                }
            }
        }

        stage('Build Installer (WiX)') {
            steps {
                echo '📦 Building installer using WiX...'
                dir("${INSTALLER_PATH}") {
                    // Assuming Installer.wixproj references the published output folder
                    bat """
                        msbuild Installer.wixproj /p:Configuration=${BUILD_CONFIG} /p:OutputPath=..\\InstallerOutput
                    """
                }
            }
        }

        stage('Archive Artifacts') {
            steps {
                echo '🗄️ Archiving installer output...'
                archiveArtifacts artifacts: 'InstallerOutput/**/*.msi', fingerprint: true
            }
        }
    }

    post {
        success {
            echo '✅ Pipeline completed successfully!'
        }
        failure {
            echo '❌ Pipeline failed!'
        }
    }
}
