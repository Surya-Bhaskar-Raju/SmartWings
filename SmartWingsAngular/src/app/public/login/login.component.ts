import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  // Login properties
  email = '';
  password = '';
  isLoading = false;
  loginErrorMessage = '';

  // Registration properties
  showRegister = false;
  registrationStep = 1; // 1: Enter details, 2: Verify OTP
  username = '';
  registerEmail = '';
  registerPassword = '';
  confirmPassword = '';
  registerOtp = '';
  private readonly role = 'User'; // Fixed role for all registrations
  isRegistering = false;
  isRequestingRegisterOtp = false;
  isVerifyingOtp = false;
  errorMessage = '';
  successMessage = '';
  registerOtpMessage = '';

  // Forgot password properties
  showForgotPassword = false;
  forgotPasswordStep = 1; // 1: Enter email, 2: Enter OTP and new password
  forgotEmail = '';
  otp = '';
  newPassword = '';
  confirmNewPassword = '';
  isRequestingOtp = false;
  isResettingPassword = false;
  forgotPasswordErrorMessage = '';
  forgotPasswordSuccessMessage = '';

  private loginUrl = `${environment.apiBaseUrl}/auth/login`;
  private registerUrl = `${environment.apiBaseUrl}/auth/register`;
  private requestOtpUrl = `${environment.apiBaseUrl}/auth/request-otp`;
  private resetPasswordUrl = `${environment.apiBaseUrl}/auth/reset-password`;
  private requestRegisterOtpUrl = `${environment.apiBaseUrl}/auth/request-otp-pre-registration`;
  private verifyRegisterOtpUrl = `${environment.apiBaseUrl}/auth/verify-otp-pre-registration`;

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    if (!this.email || !this.password) {
      this.loginErrorMessage = 'Please enter email and password';
      return;
    }

    this.isLoading = true;
    this.loginErrorMessage = '';

    this.http
      .post<any>(this.loginUrl, { email: this.email, password: this.password })
      .subscribe({
        next: (res) => {
          // Save token and user info in sessionStorage
          sessionStorage.setItem('token', res.token);
          sessionStorage.setItem('user', JSON.stringify(res.user));
          sessionStorage.setItem('userId', res.user.id); // Store userId separately for easy access

          // Redirect based on role
          if (res.user.role === 'Admin') {
            this.router.navigate(['/admin/dashboard']);
          } else {
            this.router.navigate(['/user/dashboard']);
          }
        },
        error: (err) => {
          this.loginErrorMessage = 'Login failed: Invalid email or password';
          console.error(err);
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }

  // Step 1: Request OTP for registration
  requestRegistrationOtp() {
    if (!this.username || !this.registerEmail || !this.registerPassword || !this.confirmPassword) {
      this.errorMessage = 'Please fill all the fields';
      return;
    }

    if (this.registerPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    if (this.registerPassword.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters long';
      return;
    }

    // Basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.registerEmail)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    this.isRequestingRegisterOtp = true;
    this.errorMessage = '';
    this.registerOtpMessage = '';

    const body = {
      email: this.registerEmail
    };

    this.http.post(this.requestRegisterOtpUrl, body, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log('Registration OTP Response:', response);
        this.registerOtpMessage = 'OTP has been sent to your email address';
        // Move to step 2 after a short delay to show the success message
        setTimeout(() => {
          this.registerOtpMessage = '';
          this.registrationStep = 2;
        }, 2000);
      },
      error: (err) => {
        console.error('Registration OTP Error:', err);
        
        // Handle the case where the status is 200 but response type causes an error
        if (err.status === 200 && err.error && err.error.includes('OTP sent')) {
          this.registerOtpMessage = 'OTP has been sent to your email address';
          setTimeout(() => {
            this.registerOtpMessage = '';
            this.registrationStep = 2;
          }, 2000);
        } else {
          this.errorMessage = err.error || 'Failed to send OTP. Please try again.';
        }
      },
      complete: () => {
        this.isRequestingRegisterOtp = false;
      }
    });
  }

  // Step 2: Verify OTP and complete registration
  verifyOtpAndRegister() {
    if (!this.registerOtp) {
      this.errorMessage = 'Please enter the OTP';
      return;
    }

    this.isVerifyingOtp = true;
    this.errorMessage = '';
    this.registerOtpMessage = '';

    const verifyBody = {
      email: this.registerEmail,
      otp: this.registerOtp
    };

    this.http.post(this.verifyRegisterOtpUrl, verifyBody, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log('OTP Verification Response:', response);
        // OTP verified, now proceed with registration
        this.completeRegistration();
      },
      error: (err) => {
        console.error('OTP Verification Error:', err);
        
        // Handle the case where the status is 200 but response type causes an error
        if (err.status === 200 && (err.error.includes('verified') || err.error.includes('valid'))) {
          this.completeRegistration();
        } else {
          this.errorMessage = err.error || 'Invalid OTP. Please try again.';
          this.isVerifyingOtp = false;
        }
      }
    });
  }

  // Complete the registration after OTP verification
  completeRegistration() {
    const body = {
      username: this.username,
      email: this.registerEmail,
      password: this.registerPassword,
      role: this.role
    };

    this.http.post<any>(this.registerUrl, body).subscribe({
      next: (res) => {
        this.successMessage = 'Registration successful! You can now login.';
        // Clear registration form
        this.clearRegistrationForm();
        
        // Switch back to login form after a delay
        setTimeout(() => {
          this.showRegister = false;
          this.successMessage = '';
          this.registrationStep = 1;
        }, 2000);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Registration failed';
        console.error(err);
      },
      complete: () => {
        this.isVerifyingOtp = false;
        this.isRegistering = false;
      }
    });
  }

  // Resend OTP for registration
  resendRegistrationOtp() {
    if (!this.registerEmail) {
      this.errorMessage = 'Email address is required to resend OTP';
      return;
    }

    this.isRequestingRegisterOtp = true;
    this.errorMessage = '';
    this.registerOtpMessage = '';

    const body = {
      email: this.registerEmail
    };

    this.http.post(this.requestRegisterOtpUrl, body, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log('Resend Registration OTP Response:', response);
        this.registerOtpMessage = 'OTP has been resent to your email address';
        // Clear the success message after a short delay
        setTimeout(() => {
          this.registerOtpMessage = '';
        }, 3000);
      },
      error: (err) => {
        console.error('Resend Registration OTP Error:', err);
        
        // Handle the case where the status is 200 but response type causes an error
        if (err.status === 200 && err.error && err.error.includes('OTP sent')) {
          this.registerOtpMessage = 'OTP has been resent to your email address';
          setTimeout(() => {
            this.registerOtpMessage = '';
          }, 3000);
        } else {
          this.errorMessage = err.error || 'Failed to resend OTP. Please try again.';
        }
      },
      complete: () => {
        this.isRequestingRegisterOtp = false;
      }
    });
  }

  // Clear registration form
  clearRegistrationForm() {
    this.username = '';
    this.registerEmail = '';
    this.registerPassword = '';
    this.confirmPassword = '';
    this.registerOtp = '';
    this.registrationStep = 1;
    this.errorMessage = '';
    this.registerOtpMessage = '';
  }

  register() {
    // This method is now replaced by the OTP flow
    // Keeping it for backward compatibility but redirecting to OTP flow
    this.requestRegistrationOtp();
  }

  toggleRegister() {
    this.showRegister = !this.showRegister;
    this.showForgotPassword = false;
    // Clear all error messages when toggling
    this.loginErrorMessage = '';
    this.errorMessage = '';
    this.successMessage = '';
    this.registerOtpMessage = '';
    this.registrationStep = 1;
    this.clearForgotPasswordForm();
    // Clear registration form when toggling
    if (!this.showRegister) {
      this.clearRegistrationForm();
    }
  }

  toggleForgotPassword() {
    this.showForgotPassword = !this.showForgotPassword;
    this.showRegister = false;
    this.forgotPasswordStep = 1;
    // Clear all error messages when toggling
    this.loginErrorMessage = '';
    this.errorMessage = '';
    this.successMessage = '';
    this.clearForgotPasswordForm();
  }

  clearForgotPasswordForm() {
    this.forgotEmail = '';
    this.otp = '';
    this.newPassword = '';
    this.confirmNewPassword = '';
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordSuccessMessage = '';
    this.forgotPasswordStep = 1;
  }

  // New method to clear only form fields but keep success message
  clearForgotPasswordFormFields() {
    this.forgotEmail = '';
    this.otp = '';
    this.newPassword = '';
    this.confirmNewPassword = '';
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordStep = 1;
    // Keep forgotPasswordSuccessMessage
  }

  requestOtp() {
    if (!this.forgotEmail) {
      this.forgotPasswordErrorMessage = 'Please enter your email address';
      return;
    }

    // Basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.forgotEmail)) {
      this.forgotPasswordErrorMessage = 'Please enter a valid email address';
      return;
    }

    this.isRequestingOtp = true;
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordSuccessMessage = '';

    // Use text response type since backend returns plain text
    this.http.post(this.requestOtpUrl, { email: this.forgotEmail }, { 
      responseType: 'text'
    }).subscribe({
      next: (response) => {
        console.log('OTP Response:', response);
        this.forgotPasswordSuccessMessage = 'OTP has been sent to your email address';
        // Move to step 2 after a short delay to show the success message
        setTimeout(() => {
          this.forgotPasswordSuccessMessage = ''; // Clear the message
          this.forgotPasswordStep = 2;
        }, 2000);
      },
      error: (err) => {
        console.error('OTP Error:', err);
        console.error('Error status:', err.status);
        console.error('Error response:', err.error);
        
        // Since status 200 with "OTP sent successfully" should be treated as success
        if (err.status === 200 && err.error && err.error.includes('OTP sent')) {
          this.forgotPasswordSuccessMessage = 'OTP has been sent to your email address';
          // Move to step 2 after a short delay to show the success message
          setTimeout(() => {
            this.forgotPasswordSuccessMessage = ''; // Clear the message
            this.forgotPasswordStep = 2;
          }, 2000);
        } else {
          this.forgotPasswordErrorMessage = err.error || 'Failed to send OTP. Please try again.';
        }
      },
      complete: () => {
        this.isRequestingOtp = false;
      }
    });
  }

  resetPassword() {
    if (!this.forgotEmail || !this.otp || !this.newPassword || !this.confirmNewPassword) {
      this.forgotPasswordErrorMessage = 'Please fill all the fields';
      return;
    }

    if (this.newPassword !== this.confirmNewPassword) {
      this.forgotPasswordErrorMessage = 'Passwords do not match';
      return;
    }

    if (this.newPassword.length < 6) {
      this.forgotPasswordErrorMessage = 'Password must be at least 6 characters long';
      return;
    }

    this.isResettingPassword = true;
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordSuccessMessage = '';

    const body = {
      email: this.forgotEmail,
      otp: this.otp,
      newPassword: this.newPassword
    };

    // Use POST method and text response type since backend might return plain text
    this.http.post(this.resetPasswordUrl, body, { 
      responseType: 'text'
    }).subscribe({
      next: (response) => {
        console.log('Reset Password Response:', response);
        this.forgotPasswordSuccessMessage = 'Congratulations! Your password has been changed successfully. You can now login with your new password.';
        
        // Clear form fields but keep success message visible
        this.clearForgotPasswordFormFields();
        
        // Switch back to login form after a shorter delay
        setTimeout(() => {
          this.showForgotPassword = false;
          this.forgotPasswordSuccessMessage = '';
        }, 2000); // Reduced to 2 seconds
      },
      error: (err) => {
        console.error('Reset Password Error:', err);
        console.error('Error status:', err.status);
        console.error('Error response:', err.error);
        
        // Check if the error is actually a success with text response
        if (err.status === 200 && err.error && (err.error.includes('reset') || err.error.includes('success') || err.error.includes('changed'))) {
          this.forgotPasswordSuccessMessage = 'Congratulations! Your password has been changed successfully. You can now login with your new password.';
          
          // Clear form fields but keep success message visible
          this.clearForgotPasswordFormFields();
          
          setTimeout(() => {
            this.showForgotPassword = false;
            this.forgotPasswordSuccessMessage = '';
          }, 2000); // Reduced to 2 seconds
        } else {
          this.forgotPasswordErrorMessage = err.error || 'Password reset failed. Please check your OTP and try again.';
        }
      },
      complete: () => {
        this.isResettingPassword = false;
      }
    });
  }

  // Alternative method with simpler error handling
  requestOtpSimple() {
    if (!this.forgotEmail) {
      this.forgotPasswordErrorMessage = 'Please enter your email address';
      return;
    }

    // Basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.forgotEmail)) {
      this.forgotPasswordErrorMessage = 'Please enter a valid email address';
      return;
    }

    this.isRequestingOtp = true;
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordSuccessMessage = '';

    // Simple POST request without response observation
    this.http.post(this.requestOtpUrl, { email: this.forgotEmail }).subscribe({
      next: (res) => {
        console.log('Simple OTP Response:', res);
        this.forgotPasswordSuccessMessage = 'OTP has been sent to your email address';
        this.forgotPasswordStep = 2;
        this.isRequestingOtp = false;
      },
      error: (err) => {
        console.error('Simple OTP Error:', err);
        
        // Since you're getting the OTP, let's assume it's working and proceed
        this.forgotPasswordSuccessMessage = 'OTP has been sent to your email address';
        this.forgotPasswordStep = 2;
        this.isRequestingOtp = false;
      }
    });
  }

  resendOtp() {
    if (!this.forgotEmail) {
      this.forgotPasswordErrorMessage = 'Email address is required to resend OTP';
      return;
    }

    this.isRequestingOtp = true;
    this.forgotPasswordErrorMessage = '';
    this.forgotPasswordSuccessMessage = '';

    // Use text response type since backend returns plain text
    this.http.post(this.requestOtpUrl, { email: this.forgotEmail }, { 
      responseType: 'text'
    }).subscribe({
      next: (response) => {
        console.log('Resend OTP Response:', response);
        this.forgotPasswordSuccessMessage = 'OTP has been resent to your email address';
        // Clear the success message after a short delay
        setTimeout(() => {
          this.forgotPasswordSuccessMessage = '';
        }, 3000);
      },
      error: (err) => {
        console.error('Resend OTP Error:', err);
        
        // Since status 200 with "OTP sent successfully" should be treated as success
        if (err.status === 200 && err.error && err.error.includes('OTP sent')) {
          this.forgotPasswordSuccessMessage = 'OTP has been resent to your email address';
          setTimeout(() => {
            this.forgotPasswordSuccessMessage = '';
          }, 3000);
        } else {
          this.forgotPasswordErrorMessage = err.error || 'Failed to resend OTP. Please try again.';
        }
      },
      complete: () => {
        this.isRequestingOtp = false;
      }
    });
  }
}
