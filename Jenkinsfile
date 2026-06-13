pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                echo 'Cloning MiniMart repository...'
            }
        }

        stage('Verify Files') {
            steps {
                bat 'dir'
            }
        }

        stage('Success') {
            steps {
                echo 'MiniMart pipeline executed successfully!'
            }
        }
    }
}
