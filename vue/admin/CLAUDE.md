# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Vue 3 + TypeScript admin dashboard project built with:
- **Vue 3** with Composition API
- **TypeScript** for type safety
- **Element Plus** UI component library
- **Pinia** for state management
- **Vite** as build tool
- **Vue Router** for routing
- **Vue I18n** for internationalization

## Development Commands

### Core Development
```bash
# Start development server
pnpm run dev

# Build for production
pnpm run build

# Preview production build
pnpm run serve
```

### Code Quality & Linting
```bash
# Run ESLint
pnpm run lint

# Fix ESLint issues
pnpm run fix

# Format code with Prettier
pnpm run lint:prettier

# Fix CSS/SCSS styles
pnpm run lint:stylelint

# Run lint-staged (pre-commit)
pnpm run lint:lint-staged
```

### Git & Commit
```bash
# Interactive commit with commitizen
pnpm run commit

# Setup husky hooks
pnpm run prepare
```

## Project Architecture

### Key Directories
- `src/api/` - API service layer and HTTP client configuration
- `src/assets/` - Static assets (images, icons, styles)
- `src/components/` - Reusable Vue components
- `src/composables/` - Composition API utilities
- `src/config/` - Application configuration and constants
- `src/directives/` - Custom Vue directives
- `src/enums/` - TypeScript enums
- `src/locales/` - Internationalization files
- `src/router/` - Vue Router configuration and guards
- `src/store/` - Pinia stores with persisted state
- `src/types/` - TypeScript type definitions
- `src/utils/` - Utility functions and helpers
- `src/views/` - Page-level Vue components

### Key Features
- **Auto-imports** - Components and composables automatically imported via unplugin
- **Route Guards** - Authentication and permission handling
- **Theme System** - Light/dark theme support with Element Plus customization
- **Persisted State** - Pinia stores with localStorage persistence
- **Internationalization** - Multi-language support with Vue I18n
- **Component Library** - Extensive ArtDesign component system

### Build Configuration
- **Vite** with optimized dependency pre-bundling
- **Auto component registration** via `unplugin-vue-components`
- **CSS Preprocessing** with SCSS and global style injection
- **Code splitting** with manual vendor chunk configuration
- **Compression** with gzip for production builds

## Development Patterns

### Component Structure
- Use Composition API with `<script setup>` syntax
- Follow Element Plus component naming conventions
- Leverage auto-imports for Vue and Element Plus APIs

### State Management
- Use Pinia stores for global state
- Implement persisted state with `pinia-plugin-persistedstate`
- Follow store naming pattern: `use{Name}Store`

### API Integration
- Centralized API service layer in `src/api/`
- Axios-based HTTP client with interceptors
- Type-safe API response handling

### Styling
- SCSS with CSS custom properties for theming
- Element Plus theme customization via CSS variables
- Mobile-responsive design patterns

## Environment Configuration

Environment variables are configured in:
- `.env` - Base environment settings
- `.env.development` - Development-specific settings
- `.env.production` - Production-specific settings

Key environment variables:
- `VITE_API_URL` - Backend API base URL
- `VITE_PORT` - Development server port
- `VITE_BASE_URL` - Application base path
- `VITE_VERSION` - Application version

## Code Quality Standards

- **ESLint** with TypeScript and Vue plugins
- **Prettier** for code formatting
- **Stylelint** for CSS/SCSS validation
- **Husky** with pre-commit hooks
- **Commitlint** for conventional commit messages

## Performance Optimizations

- **Vite** for fast development and optimized builds
- **Code splitting** with manual vendor chunks
- **Tree shaking** for unused code elimination
- **Image optimization** with vite-plugin-imagemin (commented)
- **Bundle analysis** with rollup-plugin-visualizer (commented)

## Browser Support

- Modern browsers with ES2020 support
- Mobile responsive design
- Touch event handling for mobile devices