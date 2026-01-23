# Remix Icons Usage

This project uses **Remix Icons** for iconography. Remix Icons is a comprehensive icon library with over 2,000 icons.

## Installation

Remix Icons should already be included in the project. If not, you can install it via:

```bash
npm install remixicon
```

Or include via CDN:
```html
<link href="https://cdn.jsdelivr.net/npm/remixicon@3.5.0/fonts/remixicon.css" rel="stylesheet">
```

## Usage in Components

All components that accept an `icon` prop use Remix Icons. The icon name should be prefixed with `ri-` and use kebab-case.

### Examples

```tsx
// Button with icon
<Button icon="ri-user-smile-line">Primary</Button>

// Alert with icon
<Alert 
  variant="primary" 
  label="Primary" 
  message="Label icon arrow alert"
  icon="ri-user-smile-line"
/>

// Tabs with icons
<Tabs 
  items={[
    {
      id: "home",
      label: "Home",
      icon: "ri-home-4-line",
      content: <div>Home content</div>
    }
  ]}
/>
```

## Common Icon Categories

### User & Account
- `ri-user-smile-line` - User smile
- `ri-user-2-line` - User
- `ri-account-circle-line` - Account circle
- `ri-profile-line` - Profile

### Navigation
- `ri-home-4-line` - Home
- `ri-menu-2-line` - Menu
- `ri-arrow-right-line` - Arrow right
- `ri-arrow-left-line` - Arrow left

### Actions
- `ri-check-double-line` - Check
- `ri-close-fill` - Close
- `ri-delete-bin-5-line` - Delete
- `ri-edit-line` - Edit
- `ri-save-line` - Save

### Communication
- `ri-mail-line` - Mail
- `ri-notification-off-line` - Notification
- `ri-message-line` - Message

### Files & Upload
- `ri-upload-cloud-2-fill` - Upload
- `ri-file-line` - File
- `ri-folder-line` - Folder

### Status
- `ri-error-warning-line` - Warning
- `ri-alert-line` - Alert
- `ri-checkbox-circle-fill` - Success

## Icon Naming Convention

Remix Icons follow this naming pattern:
- `ri-{name}-{style}`
- Styles: `line`, `fill`, `-line`, `-fill`

Examples:
- `ri-user-line` - Line style
- `ri-user-fill` - Fill style

## Full Icon List

For a complete list of available icons, visit:
https://remixicon.com/

## Integration with Components

All components in this project that display icons accept the `icon` prop as a string containing the full Remix Icon class name (e.g., `ri-home-4-line`).

The icons are rendered using `<i className={icon}></i>` in the components.
