pipeline {
    agent { label 'built-in' }  // Force the pipeline to run on the master node

    environment {
        SONARQUBE_SERVER = 'sonarcloud'
        PROJECT_KEY      = 'dotnet-calculator2'
        ORGANIZATION     = 'pnaresh479'
        BUILD_CONFIG     = 'Release'
        APP_PATH         = 'CalculatorApp'
        INSTALLER_PATH   = 'Installer'
        OUTPUT_DIR       = 'output'
        WIX_PATH         = "C:/users/admin/.dotnet/tools"
        DOTNET           = "C:/Program Files/dotnet/sdk"
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
                    bat "dotnet publish -c ${BUILD_CONFIG} -r win-x64 --self-contained true"
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                echo '🔍 Running SonarCloud analysis...'
                
                // 👇 Bind your actual Jenkins credential ID here
                withCredentials([string(credentialsId: 'sonarcloud-token-jenkins', variable: 'SONAR_TOKEN')]) {
                    withSonarQubeEnv("${SONARQUBE_SERVER}") {
                        dir("${APP_PATH}") {
                            withEnv(["PATH=${env.PATH};${env.DOTNET};C:/Users/admin/.dotnet/tools"]) {
                                bat 'dotnet sonarscanner begin /k:"${PROJECT_KEY}" /o:"${ORGANIZATION}" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login=${SONAR_TOKEN}'
                                bat "dotnet build --configuration ${BUILD_CONFIG}"
                                bat "dotnet sonarscanner end /d:sonar.login=${SONAR_TOKEN}"
                            }
                        }
                    }
                }
            }
        }

        stage('Build Installer (WiX)') {
            steps {
                echo '📦 Building installer using WiX...'
                dir("${INSTALLER_PATH}") {
                    withEnv(["PATH=${env.PATH};${env.WIX_PATH}"]) {
                        bat """
                            wix build CalculatorApp.wxs -o CalculatorApp.msi
                        """
                    }
                }
            }
        }

        stage('Archive Artifacts') {
            steps {
                echo '🗄️ Archiving installer output...'
                dir("${INSTALLER_PATH}") {
                    archiveArtifacts artifacts: 'CalculatorApp.msi', fingerprint: true
                }
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
